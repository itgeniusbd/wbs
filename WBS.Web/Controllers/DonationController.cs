using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.Services;
using Account = WBS.Web.Models.Account;

namespace WBS.Web.Controllers
{
    public class DonationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DonationController> _logger;
        private readonly ISmsService _smsService;
        private readonly IEmailService _emailService;
        private readonly IAccountService _accountService;
        private readonly ISSLCommerzService _sslCommerzService;
        private readonly IConfiguration _configuration;

        public DonationController(
            ApplicationDbContext context,
            ILogger<DonationController> logger,
            ISmsService smsService,
            IEmailService emailService,
            IAccountService accountService,
            ISSLCommerzService sslCommerzService,
            IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _smsService = smsService;
            _emailService = emailService;
            _accountService = accountService;
            _sslCommerzService = sslCommerzService;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index(int? typeId = null, int? appealId = null, decimal? amount = null, string type = null, string tier = null, string name = null, string email = null, string phone = null, string address = null)
        {
            try
            {
                _logger.LogInformation("Loading donation page...");

                // Load donation types without navigation properties
                var donationTypes = await _context.DonationTypes
                    .AsNoTracking()
                    .Where(dt => dt.IsActive)
                    .OrderBy(dt => dt.DisplayOrder)
                    .Select(dt => new DonationType
                    {
                        Id = dt.Id,
                        Name = dt.Name,
                        NameBn = dt.NameBn,
                        Description = dt.Description,
                        DescriptionBn = dt.DescriptionBn,
                        Icon = dt.Icon,
                        Image = dt.Image,
                        IsActive = dt.IsActive,
                        DisplayOrder = dt.DisplayOrder
                    })
                    .ToListAsync();

                _logger.LogInformation("Loaded {Count} donation types", donationTypes.Count);

                // Load appeals without navigation properties
                var appeals = await _context.Appeals
                    .AsNoTracking()
                    .Where(a => a.IsActive)
                    .OrderByDescending(a => a.CreatedAt)
                    .Take(10)
                    .Select(a => new Appeal
                    {
                        Id = a.Id,
                        Title = a.Title,
                        TitleBn = a.TitleBn,
                        Slug = a.Slug,
                        Summary = a.Summary,
                        FeaturedImage = a.FeaturedImage,
                        TargetAmount = a.TargetAmount,
                        RaisedAmount = a.RaisedAmount,
                        IsUrgent = a.IsUrgent
                    })
                    .ToListAsync();

                _logger.LogInformation("Loaded {Count} appeals", appeals.Count);

                // Load active accounts (optional - handle if table doesn't exist)
                List<Account> accounts = new List<Account>();
                try
                {
                    accounts = await _context.Accounts
                        .AsNoTracking()
                        .Where(a => a.IsActive)
                        .OrderBy(a => a.AccountName)
                        .ToListAsync();
                    _logger.LogInformation("Loaded {Count} accounts", accounts.Count);
                }
                catch (Exception accountEx)
                {
                    _logger.LogWarning(accountEx, "Could not load accounts. Table may not exist yet.");
                    accounts = new List<Account>();
                }

                ViewBag.DonationTypes = donationTypes;
                ViewBag.Appeals = appeals;
                ViewBag.Accounts = accounts;

                var donation = new Donation();

                if (typeId.HasValue)
                    donation.DonationTypeId = typeId.Value;

                if (appealId.HasValue)
                    donation.AppealId = appealId.Value;

                if (amount.HasValue)
                    donation.Amount = amount.Value;

                // Handle donor information from URL parameters
                if (!string.IsNullOrEmpty(name))
                    donation.DonorName = name;

                if (!string.IsNullOrEmpty(email))
                    donation.Email = email;

                if (!string.IsNullOrEmpty(phone))
                    donation.Phone = phone;

                if (!string.IsNullOrEmpty(address))
                    donation.Address = address;

                // Handle donor type from URL parameter
                if (!string.IsNullOrEmpty(type))
                {
                    // Map URL type parameter to DonorType
                    switch (type.ToLower())
                    {
                        case "lifetime":
                            donation.DonorType = "Lifetime";
                            break;
                        case "monthly":
                            donation.DonorType = "Monthly";
                            break;
                        case "daily":
                            donation.DonorType = "Daily";
                            break;
                        case "yearly":
                            donation.DonorType = "Yearly";
                            break;
                        case "regular":
                            donation.DonorType = "Regular";
                            break;
                        case "corporate":
                            donation.DonorType = "Corporate";
                            break;
                        case "onetime":
                            donation.DonorType = "OneTime";
                            break;
                    }
                }

                // Store tier information in ViewBag for display
                if (!string.IsNullOrEmpty(tier))
                {
                    ViewBag.SelectedTier = tier;
                }

                _logger.LogInformation("Donation page loaded successfully");
                return View(donation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading donation page: {Message}", ex.Message);
                TempData["Error"] = "Unable to load donation page. Please try again.";

                // Return with empty data
                ViewBag.DonationTypes = new List<DonationType>();
                ViewBag.Appeals = new List<Appeal>();
                ViewBag.Accounts = new List<Account>();
                return View(new Donation());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(Donation donation)
        {
            try
            {
                _logger.LogInformation("=== Donation Submission Started ===");
                _logger.LogInformation("DonorName: {DonorName}, Email: {Email}, Amount: {Amount}, DonationTypeId: {TypeId}, PaymentMethod: {Payment}, AccountId: {AccountId}",
                    donation.DonorName, donation.Email, donation.Amount, donation.DonationTypeId, donation.PaymentMethod, donation.AccountId);

                // Remove SDGId and ProgramId from validation since they're now optional
                ModelState.Remove("SDGId");
                ModelState.Remove("ProgramId");

                // Validate PaymentMethod explicitly
                if (string.IsNullOrWhiteSpace(donation.PaymentMethod))
                {
                    ModelState.AddModelError("PaymentMethod", "Please select a payment method");
                    _logger.LogWarning("PaymentMethod is empty");
                }

                // Validate DonorType
                if (string.IsNullOrWhiteSpace(donation.DonorType))
                {
                    ModelState.AddModelError("DonorType", "Please select a donor type");
                    _logger.LogWarning("DonorType is empty");
                }

                // Additional validation for minimum amount
                if (donation.Amount < 10)
                {
                    ModelState.AddModelError("Amount", "Minimum donation amount is ?10");
                }

                // Validate DonationTypeId
                if (donation.DonationTypeId <= 0)
                {
                    ModelState.AddModelError("DonationTypeId", "Please select a donation type");
                    _logger.LogWarning("DonationTypeId is invalid: {TypeId}", donation.DonationTypeId);
                }

                // Check if DonationType exists
                var donationTypeExists = await _context.DonationTypes
                    .AnyAsync(dt => dt.Id == donation.DonationTypeId && dt.IsActive);

                if (!donationTypeExists)
                {
                    ModelState.AddModelError("DonationTypeId", "Selected donation type is not valid");
                    _logger.LogWarning("DonationTypeId {TypeId} does not exist or is not active", donation.DonationTypeId);
                }

                // Automatically set AccountId to default/main account for public donations
                if (!donation.AccountId.HasValue || donation.AccountId.Value <= 0)
                {
                    var defaultAccount = await _context.Accounts
                        .Where(a => a.IsActive && a.Default_Status)
                        .OrderBy(a => a.Id)
                        .FirstOrDefaultAsync();

                    if (defaultAccount != null)
                    {
                        donation.AccountId = defaultAccount.Id;
                        _logger.LogInformation("Auto-selected default account {AccountId} for public donation", defaultAccount.Id);
                    }
                    else
                    {
                        // If no default account, use the first active account
                        var firstAccount = await _context.Accounts
                            .Where(a => a.IsActive)
                            .OrderBy(a => a.Id)
                            .FirstOrDefaultAsync();

                        if (firstAccount != null)
                        {
                            donation.AccountId = firstAccount.Id;
                            _logger.LogInformation("No default account found, using first active account {AccountId}", firstAccount.Id);
                        }
                        else
                        {
                            _logger.LogError("No active accounts found in the system");
                            ModelState.AddModelError("", "System configuration error. Please contact administrator.");
                        }
                    }
                }

                // Log all model state errors
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("=== Model validation failed ===");
                    foreach (var key in ModelState.Keys)
                    {
                        var errors = ModelState[key]?.Errors;
                        if (errors != null && errors.Any())
                        {
                            foreach (var error in errors)
                            {
                                _logger.LogWarning("Field: {Field}, Error: {Error}", key, error.ErrorMessage);
                            }
                        }
                    }
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _logger.LogInformation("Model is valid, creating donation...");

                        donation.Status = DonationStatus.Pending;
                        donation.CreatedAt = DateTime.UtcNow;
                        donation.TransactionId = GenerateTransactionId();

                        // Set SDGId and ProgramId to null to avoid foreign key issues
                        donation.SDGId = null;
                        donation.ProgramId = null;

                        // Ensure PaymentMethod is trimmed
                        donation.PaymentMethod = donation.PaymentMethod?.Trim() ?? string.Empty;

                        _logger.LogInformation("Adding donation to context...");
                        _context.Donations.Add(donation);

                        _logger.LogInformation("Saving changes to database...");
                        await _context.SaveChangesAsync();

                        _logger.LogInformation("Donation created successfully with ID {Id}", donation.Id);

                        // Check if payment method is online (SSLCommerz)
                        if (donation.PaymentMethod.Equals("Online", StringComparison.OrdinalIgnoreCase) ||
                            donation.PaymentMethod.Equals("SSLCommerz", StringComparison.OrdinalIgnoreCase))
                        {
                            _logger.LogInformation("Initiating SSLCommerz payment for donation {Id}", donation.Id);
                            return await InitiateSSLCommerzPayment(donation);
                        }

                        // For manual payment methods (Bank, Nagad, bKash)
                        // Create account transaction
                        try
                        {
                            if (donation.AccountId.HasValue)
                            {
                                _logger.LogInformation("Creating account transaction for donation {Id}", donation.Id);
                                await _accountService.UpdateAccountBalanceAsync(
                                    accountId: donation.AccountId.Value,
                                    amount: donation.Amount,
                                    transactionType: "Income",
                                    description: $"Donation from {donation.DonorName} - {donation.TransactionId}",
                                    referenceType: "Donation",
                                    referenceId: donation.Id
                                );
                                _logger.LogInformation("Account transaction created successfully");
                            }
                        }
                        catch (Exception txnEx)
                        {
                            _logger.LogError(txnEx, "Error creating account transaction for donation {Id}", donation.Id);
                        }

                        // Send notifications (don't fail donation if notifications fail)
                        try
                        {
                            _logger.LogInformation("Starting to send notifications...");
                            // await SendDonationNotificationsAsync(donation);
                            // Notification functionality will be implemented later
                            _logger.LogInformation("Notification skipped (feature pending implementation)");
                        }
                        catch (Exception notificationEx)
                        {
                            _logger.LogError(notificationEx, "Error sending donation notifications, but donation was saved successfully");
                        }

                        _logger.LogInformation("Redirecting to ThankYou page...");
                        return RedirectToAction(nameof(ThankYou), new { id = donation.Id });
                    }
                    catch (DbUpdateException dbEx)
                    {
                        _logger.LogError(dbEx, "Database error creating donation. InnerException: {InnerException}",
                            dbEx.InnerException?.Message);
                        TempData["Error"] = "An error occurred while processing your donation. Please try again.";
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Unexpected error creating donation: {Message}, StackTrace: {StackTrace}",
                            ex.Message, ex.StackTrace);
                        TempData["Error"] = "An error occurred while processing your donation. Please try again.";
                    }
                }
                else
                {
                    _logger.LogWarning("Model validation failed, showing form again");
                    TempData["Error"] = "Please fill all required fields correctly.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fatal error in donation submission: {Message}", ex.Message);
                TempData["Error"] = "An unexpected error occurred. Please try again.";
            }

            // Reload data for view
            try
            {
                _logger.LogInformation("Reloading donation types and appeals...");
                ViewBag.DonationTypes = await _context.DonationTypes
                    .AsNoTracking()
                    .Where(dt => dt.IsActive)
                    .OrderBy(dt => dt.DisplayOrder)
                    .Select(dt => new DonationType
                    {
                        Id = dt.Id,
                        Name = dt.Name,
                        NameBn = dt.NameBn,
                        Icon = dt.Icon
                    })
                    .ToListAsync();

                ViewBag.Appeals = await _context.Appeals
                    .AsNoTracking()
                    .Where(a => a.IsActive)
                    .OrderByDescending(a => a.CreatedAt)
                    .Take(10)
                    .Select(a => new Appeal
                    {
                        Id = a.Id,
                        Title = a.Title,
                        Slug = a.Slug
                    })
                    .ToListAsync();

                ViewBag.Accounts = await _context.Accounts
                    .AsNoTracking()
                    .Where(a => a.IsActive)
                    .OrderBy(a => a.AccountName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reloading donation types and appeals");
                ViewBag.DonationTypes = new List<DonationType>();
                ViewBag.Appeals = new List<Appeal>();
                ViewBag.Accounts = new List<Account>();
            }

            return View(donation);
        }

        public async Task<IActionResult> ThankYou(int id)
        {
            try
            {
                _logger.LogInformation("Loading thank you page for donation {Id}", id);

                var donation = await _context.Donations
                    .AsNoTracking()
                    .Include(d => d.DonationType)
                    .Include(d => d.Appeal)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (donation == null)
                {
                    _logger.LogWarning("Donation {Id} not found", id);
                    return NotFound();
                }

                _logger.LogInformation("Thank you page loaded successfully");
                return View(donation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading thank you page for donation {Id}", id);
                TempData["Error"] = "Unable to load confirmation. Please contact us with your donation details.";
                return RedirectToAction(nameof(Index));
            }
        }

        // Test action to verify donation page accessibility
        public IActionResult Test()
        {
            try
            {
                _logger.LogInformation("Test action called - Donation system is accessible");
                return Content("Donation system is working! Controller loaded successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Test action failed");
                return Content($"Error: {ex.Message}");
            }
        }

        public IActionResult JQueryTest()
        {
            return View();
        }

        #region Helper Methods

        private string GenerateTransactionId()
        {
            return $"WBS{DateTime.UtcNow:yyyyMMddHHmmss}{new Random().Next(1000, 9999)}";
        }

        private async Task<IActionResult> InitiateSSLCommerzPayment(Donation donation)
        {
            try
            {
                _logger.LogInformation("=== SSLCommerz Payment Initiation Started ===");
                _logger.LogInformation("Donation ID: {DonationId}, Amount: {Amount}, Donor: {DonorName}",
                    donation.Id, donation.Amount, donation.DonorName);

                // Get base URL - use production URL from config if available, otherwise use current request
                var productionUrl = _configuration.GetValue<string>("SSLCommerz:ProductionUrl");
                var baseUrl = !string.IsNullOrEmpty(productionUrl) && _configuration.GetValue<bool>("SSLCommerz:IsLive")
                    ? productionUrl
                    : $"{Request.Scheme}://{Request.Host}";

                // For localhost with live mode, use production URL if configured
                var isLocalhost = Request.Host.Host.Contains("localhost") || Request.Host.Host.Contains("127.0.0.1");
                var isLiveMode = _configuration.GetValue<bool>("SSLCommerz:IsLive");

                if (isLocalhost && isLiveMode)
                {
                    _logger.LogWarning("⚠️ WARNING: Using localhost with LIVE SSLCommerz mode!");
                    
                    if (!string.IsNullOrEmpty(productionUrl))
                    {
                        _logger.LogInformation("✅ Using production URL for callbacks: {ProductionUrl}", productionUrl);
                        baseUrl = productionUrl;
                    }
                    else
                    {
                        _logger.LogWarning("⚠️ Callbacks may not work properly. Consider setting ProductionUrl in appsettings or deploy to server.");
                        TempData["Warning"] = "⚠️ Testing live mode on localhost. Callbacks may not work. Please check SSLCommerz dashboard and update donation status manually if needed.";
                    }
                }

                _logger.LogInformation("Base URL for SSLCommerz callbacks: {BaseUrl}", baseUrl);

                // Ensure phone number is in proper format
                var phoneNumber = donation.Phone ?? "01700000000";
                if (!phoneNumber.StartsWith("+"))
                {
                    phoneNumber = phoneNumber.StartsWith("0") ? "+88" + phoneNumber : "+880" + phoneNumber;
                }
                _logger.LogInformation("Formatted phone number: {Phone}", phoneNumber);

                var transactionId = donation.TransactionId ?? GenerateTransactionId();
                _logger.LogInformation("Transaction ID: {TransactionId}", transactionId);

                var sslRequest = new SSLCommerzRequest
                {
                    total_amount = donation.Amount,
                    tran_id = transactionId,
                    success_url = $"{baseUrl}/Donation/PaymentSuccess",
                    fail_url = $"{baseUrl}/Donation/PaymentFail",
                    cancel_url = $"{baseUrl}/Donation/PaymentCancel",
                    ipn_url = $"{baseUrl}/Donation/PaymentIPN",

                    cus_name = !string.IsNullOrWhiteSpace(donation.DonorName) ? donation.DonorName : "Anonymous Donor",
                    cus_email = !string.IsNullOrWhiteSpace(donation.Email) ? donation.Email : "donor@wbs-bd.org",
                    cus_add1 = !string.IsNullOrWhiteSpace(donation.Address) ? donation.Address : "Dhaka, Bangladesh",
                    cus_add2 = !string.IsNullOrWhiteSpace(donation.Address) ? donation.Address : "Dhaka, Bangladesh",
                    cus_city = "Dhaka",
                    cus_state = "Dhaka",
                    cus_postcode = "1000",
                    cus_country = "Bangladesh",
                    cus_phone = phoneNumber,
                    cus_fax = phoneNumber,

                    shipping_method = "NO",
                    ship_name = !string.IsNullOrWhiteSpace(donation.DonorName) ? donation.DonorName : "Anonymous Donor",
                    ship_add1 = !string.IsNullOrWhiteSpace(donation.Address) ? donation.Address : "Dhaka, Bangladesh",
                    ship_add2 = !string.IsNullOrWhiteSpace(donation.Address) ? donation.Address : "Dhaka, Bangladesh",
                    ship_city = "Dhaka",
                    ship_state = "Dhaka",
                    ship_postcode = "1000",
                    ship_country = "Bangladesh",

                    product_name = "Donation to WBS Bangladesh",
                    product_category = "Donation",
                    product_profile = "general",

                    value_a = donation.Id.ToString()
                };

                _logger.LogInformation("SSLCommerz Request prepared: Amount={Amount}, TransactionId={TransactionId}, Customer={CustomerName}",
                    sslRequest.total_amount, sslRequest.tran_id, sslRequest.cus_name);

                _logger.LogInformation("Calling SSLCommerz API...");
                var response = await _sslCommerzService.InitiatePaymentAsync(sslRequest);

                _logger.LogInformation("SSLCommerz Response received: Status={Status}, GatewayURL={GatewayURL}",
                    response.status, response.GatewayPageURL ?? "NULL");

                if (response.status == "SUCCESS" && !string.IsNullOrEmpty(response.GatewayPageURL))
                {
                    _logger.LogInformation("✅ Payment session created successfully! Redirecting to: {GatewayURL}", response.GatewayPageURL);
                    _logger.LogInformation("=== SSLCommerz Payment Initiation Completed Successfully ===");

                    // Update transaction ID if it was generated
                    if (string.IsNullOrEmpty(donation.TransactionId))
                    {
                        donation.TransactionId = transactionId;
                        await _context.SaveChangesAsync();
                    }

                    // Store in TempData for localhost fallback
                    if (isLocalhost && isLiveMode)
                    {
                        TempData["PendingDonationId"] = donation.Id;
                        TempData["PendingTransactionId"] = transactionId;
                    }

                    return Redirect(response.GatewayPageURL);
                }
                else
                {
                    var errorReason = !string.IsNullOrEmpty(response.failedreason)
                        ? response.failedreason
                        : "Unknown error - no gateway URL returned";

                    _logger.LogError("❌ SSLCommerz payment initiation failed!");
                    _logger.LogError("Status: {Status}", response.status);
                    _logger.LogError("Failed Reason: {Reason}", errorReason);
                    _logger.LogError("Gateway URL: {GatewayURL}", response.GatewayPageURL ?? "NULL");
                    _logger.LogError("Session Key: {SessionKey}", response.sessionkey ?? "NULL");

                    donation.Status = DonationStatus.Failed;
                    await _context.SaveChangesAsync();

                    // User-friendly error messages
                    if (errorReason.Contains("STORE_ID") || errorReason.Contains("credentials"))
                    {
                        TempData["Error"] = "Payment gateway configuration error. Please contact support or use manual payment methods.";
                        _logger.LogError("⚠️ Credentials issue detected. Please verify SSLCommerz Store ID and Password.");
                    }
                    else if (errorReason.Contains("amount") || errorReason.Contains("invalid"))
                    {
                        TempData["Error"] = "Invalid payment amount. Please try again or use manual payment methods.";
                    }
                    else
                    {
                        TempData["Error"] = $"Unable to process online payment at this moment. Please use manual payment methods (bKash, Nagad, or Bank).";
                        _logger.LogError("Full error details: {ErrorReason}", errorReason);
                    }

                    _logger.LogInformation("=== SSLCommerz Payment Initiation Failed ===");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "💥 Exception in SSLCommerz payment initiation");
                _logger.LogError("Exception Type: {ExceptionType}", ex.GetType().Name);
                _logger.LogError("Exception Message: {Message}", ex.Message);
                _logger.LogError("Stack Trace: {StackTrace}", ex.StackTrace);

                TempData["Error"] = "An error occurred while processing your payment. Please try using manual payment methods (bKash, Nagad, or Bank).";

                _logger.LogInformation("=== SSLCommerz Payment Initiation Failed with Exception ===");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> PaymentSuccess()
        {
            try
            {
                var valId = Request.Form["val_id"].ToString();
                var tranId = Request.Form["tran_id"].ToString();
                var amount = decimal.Parse(Request.Form["amount"].ToString());
                var status = Request.Form["status"].ToString();

                _logger.LogInformation("=== Payment Success Callback Received ===");
                _logger.LogInformation("Transaction ID: {TransactionId}, Validation ID: {ValidationId}, Amount: {Amount}, Status: {Status}",
                    tranId, valId, amount, status);

                // Find donation by transaction ID
                var donation = await _context.Donations.FirstOrDefaultAsync(d => d.TransactionId == tranId);

                if (donation == null)
                {
                    _logger.LogWarning("Donation not found for transaction ID: {TransactionId}", tranId);
                    TempData["Error"] = "Donation record not found. Please contact support with your transaction ID: " + tranId;
                    return RedirectToAction(nameof(Index));
                }

                // For sandbox/test mode, we can skip validation if it fails
                var isTestMode = _configuration.GetValue<bool>("SSLCommerz:IsLive") == false;

                if (isTestMode)
                {
                    _logger.LogInformation("Test mode detected - accepting payment without strict validation");

                    // Update donation status
                    donation.Status = DonationStatus.Completed;
                    donation.PaymentStatus = "Paid";
                    donation.PaymentDate = DateTime.UtcNow;
                    donation.PaidAt = DateTime.UtcNow;
                    donation.BankTransactionId = Request.Form["bank_tran_id"].ToString();
                    donation.CardType = Request.Form["card_type"].ToString();

                    await _context.SaveChangesAsync();

                    _logger.LogInformation("✅ Donation {Id} marked as completed (Test Mode)", donation.Id);

                    // Update Appeal RaisedAmount
                    if (donation.AppealId.HasValue)
                    {
                        var appeal = await _context.Appeals.FindAsync(donation.AppealId.Value);
                        if (appeal != null)
                        {
                            appeal.RaisedAmount += donation.Amount;
                            appeal.UpdatedAt = DateTime.UtcNow;
                            _logger.LogInformation("✅ Appeal {AppealId} raised amount updated: +৳{Amount}", donation.AppealId.Value, donation.Amount);
                        }
                    }

                    // Create account transaction
                    if (donation.AccountId.HasValue)
                    {
                        try
                        {
                            await _accountService.UpdateAccountBalanceAsync(
                                accountId: donation.AccountId.Value,
                                amount: donation.Amount,
                                transactionType: "Income",
                                description: $"Online donation from {donation.DonorName} - {donation.TransactionId}",
                                referenceType: "Donation",
                                referenceId: donation.Id
                            );
                            _logger.LogInformation("✅ Account balance updated successfully");
                        }
                        catch (Exception accEx)
                        {
                            _logger.LogError(accEx, "Error updating account balance");
                        }
                    }

                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(ThankYou), new { id = donation.Id });
                }

                // For live mode, validate with SSLCommerz
                try
                {
                    _logger.LogInformation("Attempting to validate payment with SSLCommerz...");
                    var validationResponse = await _sslCommerzService.ValidatePaymentAsync(valId);
                    _logger.LogInformation("Validation response status: {Status}", validationResponse.status);

                    if (validationResponse.status == "VALID" || validationResponse.status == "VALIDATED")
                    {
                        donation.Status = DonationStatus.Completed;
                        donation.PaymentStatus = "Paid";
                        donation.PaymentDate = DateTime.UtcNow;
                        donation.PaidAt = DateTime.UtcNow;
                        donation.BankTransactionId = Request.Form["bank_tran_id"].ToString();
                        donation.CardType = Request.Form["card_type"].ToString();

                        await _context.SaveChangesAsync();

                        _logger.LogInformation("✅ Donation {Id} marked as completed (Validated)", donation.Id);

                        // Update Appeal RaisedAmount
                        if (donation.AppealId.HasValue)
                        {
                            var appeal = await _context.Appeals.FindAsync(donation.AppealId.Value);
                            if (appeal != null)
                            {
                                appeal.RaisedAmount += donation.Amount;
                                appeal.UpdatedAt = DateTime.UtcNow;
                                _logger.LogInformation("✅ Appeal {AppealId} raised amount updated: +৳{Amount}", donation.AppealId.Value, donation.Amount);
                            }
                        }

                        // Create account transaction
                        if (donation.AccountId.HasValue)
                        {
                            await _accountService.UpdateAccountBalanceAsync(
                                accountId: donation.AccountId.Value,
                                amount: donation.Amount,
                                transactionType: "Income",
                                description: $"Online donation from {donation.DonorName} - {donation.TransactionId}",
                                referenceType: "Donation",
                                referenceId: donation.Id
                            );
                            _logger.LogInformation("✅ Account balance updated successfully");
                        }

                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(ThankYou), new { id = donation.Id });
                    }
                    else
                    {
                        _logger.LogWarning("⚠️ Validation returned status: {Status}", validationResponse.status);
                        
                        // Even if validation returns non-VALID status, check the callback status
                        if (status == "VALID" || status == "VALIDATED")
                        {
                            _logger.LogInformation("Callback status is VALID, accepting payment despite validation API response");
                            
                            donation.Status = DonationStatus.Completed;
                            donation.PaymentStatus = "Paid";
                            donation.PaymentDate = DateTime.UtcNow;
                            donation.PaidAt = DateTime.UtcNow;
                            donation.BankTransactionId = Request.Form["bank_tran_id"].ToString();
                            donation.CardType = Request.Form["card_type"].ToString();

                            await _context.SaveChangesAsync();

                            _logger.LogInformation("✅ Donation {Id} marked as completed (Callback Valid)", donation.Id);

                            // Update Appeal RaisedAmount
                            if (donation.AppealId.HasValue)
                            {
                                var appeal = await _context.Appeals.FindAsync(donation.AppealId.Value);
                                if (appeal != null)
                                {
                                    appeal.RaisedAmount += donation.Amount;
                                    appeal.UpdatedAt = DateTime.UtcNow;
                                    _logger.LogInformation("✅ Appeal {AppealId} raised amount updated: +৳{Amount}", donation.AppealId.Value, donation.Amount);
                                }
                            }

                            // Create account transaction
                            if (donation.AccountId.HasValue)
                            {
                                await _accountService.UpdateAccountBalanceAsync(
                                    accountId: donation.AccountId.Value,
                                    amount: donation.Amount,
                                    transactionType: "Income",
                                    description: $"Online donation from {donation.DonorName} - {donation.TransactionId}",
                                    referenceType: "Donation",
                                    referenceId: donation.Id
                                );
                            }

                            await _context.SaveChangesAsync();

                            return RedirectToAction(nameof(ThankYou), new { id = donation.Id });
                        }
                    }
                }
                catch (Exception valEx)
                {
                    _logger.LogError(valEx, "Validation API error - but payment callback received");

                    // If validation fails but we received success callback with VALID/VALIDATED status
                    if (status == "VALID" || status == "VALIDATED")
                    {
                        _logger.LogWarning("⚠️ Validation API failed but callback status is {Status}, accepting payment", status);

                        donation.Status = DonationStatus.Completed;
                        donation.PaymentStatus = "Paid";
                        donation.PaymentDate = DateTime.UtcNow;
                        donation.PaidAt = DateTime.UtcNow;
                        donation.BankTransactionId = Request.Form["bank_tran_id"].ToString();
                        donation.CardType = Request.Form["card_type"].ToString();

                        await _context.SaveChangesAsync();

                        _logger.LogInformation("✅ Donation {Id} marked as completed (Callback Valid, Validation Failed)", donation.Id);

                        // Update Appeal RaisedAmount
                        if (donation.AppealId.HasValue)
                        {
                            var appeal = await _context.Appeals.FindAsync(donation.AppealId.Value);
                            if (appeal != null)
                            {
                                appeal.RaisedAmount += donation.Amount;
                                appeal.UpdatedAt = DateTime.UtcNow;
                                _logger.LogInformation("✅ Appeal {AppealId} raised amount updated: +৳{Amount}", donation.AppealId.Value, donation.Amount);
                            }
                        }

                        // Create account transaction
                        if (donation.AccountId.HasValue)
                        {
                            try
                            {
                                await _accountService.UpdateAccountBalanceAsync(
                                    accountId: donation.AccountId.Value,
                                    amount: donation.Amount,
                                    transactionType: "Income",
                                    description: $"Online donation from {donation.DonorName} - {donation.TransactionId}",
                                    referenceType: "Donation",
                                    referenceId: donation.Id
                                );
                            }
                            catch (Exception accEx)
                            {
                                _logger.LogError(accEx, "Error updating account balance");
                            }
                        }

                        await _context.SaveChangesAsync();

                        return RedirectToAction(nameof(ThankYou), new { id = donation.Id });
                    }
                    else
                    {
                        _logger.LogError("❌ Validation failed AND callback status is not VALID: {Status}", status);
                        
                        // Mark as pending verification for manual review
                        donation.Status = DonationStatus.Pending;
                        donation.PaymentStatus = "Pending Verification";
                        donation.PaymentDate = DateTime.UtcNow;
                        donation.BankTransactionId = Request.Form["bank_tran_id"].ToString();
                        donation.CardType = Request.Form["card_type"].ToString();

                        await _context.SaveChangesAsync();

                        _logger.LogInformation("⚠️ Donation {Id} marked as pending verification", donation.Id);

                        TempData["Warning"] = "Payment received! Your donation is being verified. You will receive confirmation shortly. Transaction ID: " + tranId;
                        return RedirectToAction(nameof(ThankYou), new { id = donation.Id });
                    }
                }

                TempData["Error"] = "Payment validation failed. Please contact support with transaction ID: " + tranId;
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment success callback");
                TempData["Error"] = "An error occurred while processing your payment confirmation.";
                return RedirectToAction(nameof(Index));
            }
        }

        // Manual verification endpoint for checking payment status
        public async Task<IActionResult> CheckPaymentStatus(string transactionId)
        {
            if (string.IsNullOrEmpty(transactionId))
            {
                TempData["Error"] = "Transaction ID required";
                return RedirectToAction(nameof(Index));
            }

            try
            {
                var donation = await _context.Donations
                    .FirstOrDefaultAsync(d => d.TransactionId == transactionId);

                if (donation == null)
                {
                    TempData["Error"] = "Donation not found for transaction ID: " + transactionId;
                    return RedirectToAction(nameof(Index));
                }

                // If already completed, just show thank you
                if (donation.Status == DonationStatus.Completed)
                {
                    return RedirectToAction(nameof(ThankYou), new { id = donation.Id });
                }

                // Try to validate manually
                _logger.LogInformation("Manual validation check for transaction: {TransactionId}", transactionId);

                // For now, show the current status
                ViewBag.Donation = donation;
                ViewBag.Message = $"Your donation (ID: {donation.Id}) is currently in '{donation.Status}' status. " +
                                 $"Please check SSLCommerz merchant panel or contact support if payment was successful.";

                return View("PaymentStatus");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking payment status");
                TempData["Error"] = "Error checking payment status";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> PaymentFail()
        {
            try
            {
                var tranId = Request.Form["tran_id"].ToString();
                var errorMessage = Request.Form["error"].ToString();

                _logger.LogWarning("Payment failed for transaction: {TransactionId}, Error: {Error}", tranId, errorMessage);

                var donation = await _context.Donations.FirstOrDefaultAsync(d => d.TransactionId == tranId);
                if (donation != null)
                {
                    donation.Status = DonationStatus.Failed;
                    donation.PaymentStatus = "Failed";
                    await _context.SaveChangesAsync();
                }

                TempData["Error"] = "Payment failed. Please try again or use manual payment methods.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment failure");
                TempData["Error"] = "Payment failed.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> PaymentCancel()
        {
            try
            {
                var tranId = Request.Form["tran_id"].ToString();

                _logger.LogInformation("Payment cancelled for transaction: {TransactionId}", tranId);

                var donation = await _context.Donations.FirstOrDefaultAsync(d => d.TransactionId == tranId);
                if (donation != null)
                {
                    donation.Status = DonationStatus.Cancelled;
                    donation.PaymentStatus = "Cancelled";
                    await _context.SaveChangesAsync();
                }

                TempData["Warning"] = "Payment was cancelled. You can try again.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment cancellation");
                TempData["Warning"] = "Payment was cancelled.";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        public async Task<IActionResult> PaymentIPN()
        {
            try
            {
                var valId = Request.Form["val_id"].ToString();
                var tranId = Request.Form["tran_id"].ToString();

                _logger.LogInformation("IPN received for transaction: {TransactionId}", tranId);

                // Validate with SSLCommerz
                var validationResponse = await _sslCommerzService.ValidatePaymentAsync(valId);

                if (validationResponse.status == "VALID" || validationResponse.status == "VALIDATED")
                {
                    var donation = await _context.Donations.FirstOrDefaultAsync(d => d.TransactionId == tranId);
                    if (donation != null && donation.Status != DonationStatus.Completed)
                    {
                        donation.Status = DonationStatus.Completed;
                        donation.PaymentStatus = "Paid";
                        donation.PaymentDate = DateTime.UtcNow;
                        donation.PaidAt = DateTime.UtcNow;
                        await _context.SaveChangesAsync();

                        _logger.LogInformation("✅ Donation {Id} updated via IPN", donation.Id);

                        // Update Appeal RaisedAmount
                        if (donation.AppealId.HasValue)
                        {
                            var appeal = await _context.Appeals.FindAsync(donation.AppealId.Value);
                            if (appeal != null)
                            {
                                appeal.RaisedAmount += donation.Amount;
                                appeal.UpdatedAt = DateTime.UtcNow;
                                _logger.LogInformation("✅ Appeal {AppealId} raised amount updated via IPN: +৳{Amount}", donation.AppealId.Value, donation.Amount);
                            }
                        }

                        // Update account balance
                        if (donation.AccountId.HasValue)
                        {
                            await _accountService.UpdateAccountBalanceAsync(
                                accountId: donation.AccountId.Value,
                                amount: donation.Amount,
                                transactionType: "Income",
                                description: $"Online donation from {donation.DonorName} - {donation.TransactionId}",
                                referenceType: "Donation",
                                referenceId: donation.Id
                            );
                        }

                        await _context.SaveChangesAsync();
                    }
                }

                return Ok("IPN processed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing IPN");
                return StatusCode(500, "Error processing IPN");
            }
        }

        #endregion
    }
}
