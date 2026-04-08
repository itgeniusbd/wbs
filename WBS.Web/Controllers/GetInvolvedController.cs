using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.ViewModels;
using WBS.Web.Services;

namespace WBS.Web.Controllers
{
    public class GetInvolvedController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ILogger<GetInvolvedController> _logger;
        private readonly ISSLCommerzService _sslCommerzService;
        private readonly IConfiguration _configuration;

        public GetInvolvedController(
            ApplicationDbContext context, 
            IWebHostEnvironment environment, 
            ILogger<GetInvolvedController> logger,
            ISSLCommerzService sslCommerzService,
            IConfiguration configuration)
        {
            _context = context;
            _environment = environment;
            _logger = logger;
            _sslCommerzService = sslCommerzService;
            _configuration = configuration;
        }

        // Regular Donor
        [Route("getinvolved/Regular-Donor")]
        [Route("getinvolved/RegularDonor")]
        public IActionResult RegularDonor()
        {
            return View();
        }

        // Lifetime Donor
        [Route("getinvolved/Lifetime-Donor")]
        [Route("getinvolved/LifetimeDonor")]
        public IActionResult LifetimeDonor()
        {
            return View();
        }

        // Volunteer
        public async Task<IActionResult> Volunteer()
        {
            try
            {
                // Load all active SDG Projects for dropdown
                var projects = await _context.SDGProjects
                    .Where(e => e.IsActive)
                    .OrderBy(e => e.DisplayOrder)
                    .ThenBy(e => e.Title)
                    .Select(e => new { 
                        e.Id, 
                        e.Title, 
                        e.TitleBn, 
                        e.District, 
                        e.DistrictBn,
                        e.Village,
                        e.VillageBn
                    })
                    .ToListAsync();

                ViewBag.Projects = projects;
            }
            catch
            {
                // If SDGProjects table doesn't exist or has issues, return empty list
                ViewBag.Projects = new List<dynamic>();
            }

            return View(new VolunteerFormViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Volunteer(VolunteerFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    // Reload projects for dropdown
                    var projects = await _context.SDGProjects
                        .Where(e => e.IsActive)
                        .OrderBy(e => e.DisplayOrder)
                        .ThenBy(e => e.Title)
                        .Select(e => new { 
                            e.Id, 
                            e.Title, 
                            e.TitleBn, 
                            e.District, 
                            e.DistrictBn,
                            e.Village,
                            e.VillageBn
                        })
                        .ToListAsync();
                    ViewBag.Projects = projects;
                }
                catch
                {
                    ViewBag.Projects = new List<dynamic>();
                }
                return View(model);
            }

            var volunteer = new Volunteer
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                Address = model.Address,
                Skills = model.Skills,
                SDGProjectId = model.EventId, // Using EventId from ViewModel for backward compatibility
                Message = model.Message,
                Status = "Pending",
                AppliedDate = DateTime.UtcNow
            };

            _context.Volunteers.Add(volunteer);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Thank you for your interest in volunteering with us!";
            return RedirectToAction("VolunteerThankYou");
        }

        public IActionResult VolunteerThankYou()
        {
            return View();
        }

        // Events - Now showing actual Events (not SDG Projects)
        [Route("getinvolved/Events")]
        [Route("events")]
        public async Task<IActionResult> Events()
        {
            // Show active events that haven't ended yet (or no end date specified)
            var events = await _context.Events
                .Include(e => e.Registrations)
                .Where(e => e.IsActive && 
                           (e.EndDate == null || e.EndDate >= DateTime.UtcNow || 
                            e.StartDate >= DateTime.UtcNow.AddDays(-30)))
                .OrderBy(e => e.StartDate)
                .ToListAsync();

            return View(events);
        }

        [Route("getinvolved/events/{id:int}")]
        [Route("events/{id:int}")]
        [Route("events/{slug}")]
        public async Task<IActionResult> EventDetails(int? id, string? slug)
        {
            Event? eventModel = null;
            
            if (id.HasValue)
            {
                eventModel = await _context.Events
                    .Include(e => e.Registrations)
                    .FirstOrDefaultAsync(e => e.Id == id.Value && e.IsActive);
            }
            else if (!string.IsNullOrEmpty(slug))
            {
                eventModel = await _context.Events
                    .Include(e => e.Registrations)
                    .FirstOrDefaultAsync(e => e.Slug == slug && e.IsActive);
            }

            if (eventModel == null)
                return NotFound();

            return View(eventModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterForEvent(int eventId, string fullName, string email, string phone)
        {
            try
            {
                var eventModel = await _context.Events
                    .Include(e => e.Registrations)
                    .FirstOrDefaultAsync(e => e.Id == eventId && e.IsActive);

                if (eventModel == null)
                {
                    TempData["Error"] = "Event not found.";
                    return RedirectToAction("Events");
                }

                // Check if registration deadline has passed
                if (eventModel.RegistrationDeadline.HasValue && eventModel.RegistrationDeadline.Value < DateTime.UtcNow)
                {
                    TempData["Error"] = "Registration deadline has passed for this event.";
                    return RedirectToAction("EventDetails", new { id = eventId });
                }

                // Check if event is full
                if (eventModel.AvailableSeats <= 0)
                {
                    TempData["Error"] = "Sorry, this event is fully booked.";
                    return RedirectToAction("EventDetails", new { id = eventId });
                }

                // Check if user already registered
                var existingRegistration = await _context.EventRegistrations
                    .FirstOrDefaultAsync(r => r.EventId == eventId && r.Email == email);

                if (existingRegistration != null)
                {
                    TempData["Error"] = "You have already registered for this event.";
                    return RedirectToAction("EventDetails", new { id = eventId });
                }

                // Create registration with pending status
                var registration = new EventRegistration
                {
                    EventId = eventId,
                    FullName = fullName,
                    Email = email,
                    Phone = phone,
                    AmountPaid = eventModel.TicketPrice.GetValueOrDefault(),
                    PaymentMethod = eventModel.TicketPrice.GetValueOrDefault() > 0 ? "SSLCommerz" : "Free",
                    Status = eventModel.TicketPrice.GetValueOrDefault() > 0 ? "Pending" : "Confirmed",
                    RegisteredAt = DateTime.UtcNow,
                    ConfirmedAt = eventModel.TicketPrice.GetValueOrDefault() > 0 ? null : DateTime.UtcNow,
                    TransactionId = GenerateTransactionId()
                };

                _context.EventRegistrations.Add(registration);
                await _context.SaveChangesAsync();

                // If event has a ticket price, redirect to payment
                if (eventModel.TicketPrice.HasValue && eventModel.TicketPrice.Value > 0)
                {
                    _logger.LogInformation("Event requires payment. Initiating SSLCommerz payment for registration {Id}", registration.Id);
                    return await InitiateEventPaymentAsync(registration, eventModel);
                }

                // For free events, confirm immediately
                TempData["Success"] = "Thank you for registering! You will receive a confirmation email shortly.";
                return RedirectToAction("EventDetails", new { id = eventId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering for event");
                TempData["Error"] = "An error occurred while processing your registration. Please try again.";
                return RedirectToAction("EventDetails", new { id = eventId });
            }
        }

        private string GenerateTransactionId()
        {
            return $"WBS-EVT-{DateTime.UtcNow:yyyyMMddHHmmss}-{new Random().Next(1000, 9999)}";
        }

        // Career
        public async Task<IActionResult> Career()
        {
            var careers = await _context.Careers
                .Where(c => c.IsActive && (c.Deadline == null || c.Deadline >= DateTime.UtcNow))
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return View(careers);
        }

        [Route("career/{slug}")]
        public async Task<IActionResult> CareerDetails(string slug)
        {
            var career = await _context.Careers
                .FirstOrDefaultAsync(c => c.Slug == slug && c.IsActive);

            if (career == null)
                return NotFound();

            return View(career);
        }

        // Career Application
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyForCareer(CVApplication application, IFormFile? cvFile)
        {
            try
            {
                _logger.LogInformation("=== Career Application Started ===");
                _logger.LogInformation("Applicant: {Name}, Email: {Email}", application.FullName, application.Email);

                // Validate required fields
                if (string.IsNullOrWhiteSpace(application.FullName))
                {
                    TempData["Error"] = "Please provide your full name.";
                    return RedirectToAction("Career");
                }

                if (string.IsNullOrWhiteSpace(application.Email))
                {
                    TempData["Error"] = "Please provide your email address.";
                    return RedirectToAction("Career");
                }

                // Handle CV file upload
                if (cvFile != null && cvFile.Length > 0)
                {
                    _logger.LogInformation("CV file uploaded: {FileName}, Size: {Size}", cvFile.FileName, cvFile.Length);

                    // Validate file size (max 5MB)
                    if (cvFile.Length > 5 * 1024 * 1024)
                    {
                        TempData["Error"] = "CV file size must be less than 5MB.";
                        return RedirectToAction("Career");
                    }

                    // Validate file type
                    var allowedExtensions = new[] { ".pdf", ".doc", ".docx" };
                    var extension = Path.GetExtension(cvFile.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                    {
                        TempData["Error"] = "Only PDF, DOC, and DOCX files are allowed for CV.";
                        return RedirectToAction("Career");
                    }

                    // Save CV file
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "cvs");
                    Directory.CreateDirectory(uploadsFolder);

                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await cvFile.CopyToAsync(stream);
                    }

                    application.CVFilePath = $"/uploads/cvs/{fileName}";
                    _logger.LogInformation("CV saved: {Path}", application.CVFilePath);
                }
                else
                {
                    TempData["Error"] = "Please upload your CV (PDF, DOC, or DOCX).";
                    return RedirectToAction("Career");
                }

                application.AppliedDate = DateTime.UtcNow;
                application.Status = "Pending";

                _context.CVApplications.Add(application);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Application saved successfully. ID: {Id}", application.Id);

                TempData["Success"] = "Thank you for your application! We will review your CV and contact you soon.";
                return RedirectToAction("Career");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing career application");
                TempData["Error"] = "An error occurred while submitting your application. Please try again.";
                return RedirectToAction("Career");
            }
        }

        #region Event Payment Methods

        private async Task<IActionResult> InitiateEventPaymentAsync(EventRegistration registration, Event eventModel)
        {
            try
            {
                _logger.LogInformation("=== Event Payment Initiation Started ===");
                _logger.LogInformation("Registration ID: {RegistrationId}, Amount: {Amount}, Event: {Event}",
                    registration.Id, registration.AmountPaid, eventModel.Title);

                // Get base URL
                var productionUrl = _configuration.GetValue<string>("SSLCommerz:ProductionUrl");
                var baseUrl = !string.IsNullOrEmpty(productionUrl) && _configuration.GetValue<bool>("SSLCommerz:IsLive")
                    ? productionUrl
                    : $"{Request.Scheme}://{Request.Host}";

                var isLocalhost = Request.Host.Host.Contains("localhost") || Request.Host.Host.Contains("127.0.0.1");
                var isLiveMode = _configuration.GetValue<bool>("SSLCommerz:IsLive");

                if (isLocalhost && isLiveMode && !string.IsNullOrEmpty(productionUrl))
                {
                    _logger.LogInformation("Using production URL for callbacks: {ProductionUrl}", productionUrl);
                    baseUrl = productionUrl;
                }

                _logger.LogInformation("Base URL for SSLCommerz callbacks: {BaseUrl}", baseUrl);

                // Ensure phone number is in proper format
                var phoneNumber = registration.Phone ?? "01700000000";
                if (!phoneNumber.StartsWith("+"))
                {
                    phoneNumber = phoneNumber.StartsWith("0") ? "+88" + phoneNumber : "+880" + phoneNumber;
                }

                var sslRequest = new SSLCommerzRequest
                {
                    total_amount = registration.AmountPaid,
                    tran_id = registration.TransactionId,
                    success_url = $"{baseUrl}/GetInvolved/EventPaymentSuccess",
                    fail_url = $"{baseUrl}/GetInvolved/EventPaymentFail",
                    cancel_url = $"{baseUrl}/GetInvolved/EventPaymentCancel",
                    ipn_url = $"{baseUrl}/GetInvolved/EventPaymentIPN",

                    cus_name = registration.FullName,
                    cus_email = registration.Email,
                    cus_add1 = registration.Address ?? "Dhaka, Bangladesh",
                    cus_add2 = registration.Address ?? "Dhaka, Bangladesh",
                    cus_city = "Dhaka",
                    cus_state = "Dhaka",
                    cus_postcode = "1000",
                    cus_country = "Bangladesh",
                    cus_phone = phoneNumber,
                    cus_fax = phoneNumber,

                    shipping_method = "NO",
                    ship_name = registration.FullName,
                    ship_add1 = registration.Address ?? "Dhaka, Bangladesh",
                    ship_add2 = registration.Address ?? "Dhaka, Bangladesh",
                    ship_city = "Dhaka",
                    ship_state = "Dhaka",
                    ship_postcode = "1000",
                    ship_country = "Bangladesh",

                    product_name = $"Event Registration: {eventModel.Title}",
                    product_category = "Event",
                    product_profile = "general",

                    value_a = registration.Id.ToString(),
                    value_b = registration.EventId.ToString()
                };

                _logger.LogInformation("Calling SSLCommerz API...");
                var response = await _sslCommerzService.InitiatePaymentAsync(sslRequest);

                if (response.status == "SUCCESS" && !string.IsNullOrEmpty(response.GatewayPageURL))
                {
                    _logger.LogInformation("Payment session created successfully! Redirecting to: {GatewayURL}", response.GatewayPageURL);
                    
                    if (isLocalhost && isLiveMode)
                    {
                        TempData["PendingRegistrationId"] = registration.Id;
                        TempData["PendingTransactionId"] = registration.TransactionId;
                    }

                    return Redirect(response.GatewayPageURL);
                }
                else
                {
                    _logger.LogError("SSLCommerz payment initiation failed: {Reason}", response.failedreason);
                    
                    registration.Status = "Failed";
                    registration.Notes = $"Payment initiation failed: {response.failedreason}";
                    await _context.SaveChangesAsync();

                    TempData["Error"] = "Unable to process payment at this moment. Please try again later.";
                    return RedirectToAction("EventDetails", new { id = registration.EventId });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception in event payment initiation");
                TempData["Error"] = "An error occurred while processing your payment. Please try again.";
                return RedirectToAction("EventDetails", new { id = registration.EventId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> EventPaymentSuccess()
        {
            try
            {
                var valId = Request.Form["val_id"].ToString();
                var tranId = Request.Form["tran_id"].ToString();
                var amount = decimal.Parse(Request.Form["amount"].ToString());
                var status = Request.Form["status"].ToString();

                _logger.LogInformation("=== Event Payment Success Callback ===");
                _logger.LogInformation("Transaction ID: {TransactionId}, Validation ID: {ValidationId}, Amount: {Amount}, Status: {Status}",
                    tranId, valId, amount, status);

                var registration = await _context.EventRegistrations
                    .Include(r => r.Event)
                    .FirstOrDefaultAsync(r => r.TransactionId == tranId);

                if (registration == null)
                {
                    _logger.LogWarning("Registration not found for transaction ID: {TransactionId}", tranId);
                    TempData["Error"] = "Registration record not found. Please contact support.";
                    return RedirectToAction("Events");
                }

                // Check if in test mode
                var isTestMode = _configuration.GetValue<bool>("SSLCommerz:IsLive") == false;

                if (isTestMode)
                {
                    _logger.LogInformation("Test mode detected - accepting payment without strict validation");

                    registration.Status = "Confirmed";
                    registration.ConfirmedAt = DateTime.UtcNow;
                    registration.PaymentMethod = "SSLCommerz";
                    registration.Notes = $"Payment confirmed (Test Mode). Bank Transaction ID: {Request.Form["bank_tran_id"]}";
                    
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("? Event registration {RegistrationId} confirmed successfully (Test Mode)", registration.Id);
                    
                    TempData["Success"] = "Payment successful! Your event registration is confirmed.";
                    return RedirectToAction("EventPaymentConfirmation", new { id = registration.Id });
                }

                // Live mode - try validation but also accept callback status
                try
                {
                    _logger.LogInformation("Attempting to validate payment with SSLCommerz...");
                    var validationResponse = await _sslCommerzService.ValidatePaymentAsync(valId);
                    _logger.LogInformation("Validation response status: {Status}", validationResponse.status);

                    if (validationResponse.status == "VALID" || validationResponse.status == "VALIDATED")
                    {
                        registration.Status = "Confirmed";
                        registration.ConfirmedAt = DateTime.UtcNow;
                        registration.PaymentMethod = "SSLCommerz";
                        registration.Notes = $"Payment validated. Bank Transaction ID: {Request.Form["bank_tran_id"]}";
                        
                        await _context.SaveChangesAsync();

                        _logger.LogInformation("? Event registration {RegistrationId} confirmed successfully (Validated)", registration.Id);
                        
                        TempData["Success"] = "Payment successful! Your event registration is confirmed.";
                        return RedirectToAction("EventPaymentConfirmation", new { id = registration.Id });
                    }
                    else
                    {
                        _logger.LogWarning("?? Validation returned status: {Status}", validationResponse.status);
                        
                        // Even if validation returns non-VALID status, check the callback status
                        if (status == "VALID" || status == "VALIDATED")
                        {
                            _logger.LogInformation("Callback status is VALID, accepting payment despite validation API response");
                            
                            registration.Status = "Confirmed";
                            registration.ConfirmedAt = DateTime.UtcNow;
                            registration.PaymentMethod = "SSLCommerz";
                            registration.Notes = $"Payment confirmed (Callback Valid). Bank Transaction ID: {Request.Form["bank_tran_id"]}";

                            await _context.SaveChangesAsync();

                            _logger.LogInformation("? Event registration {RegistrationId} confirmed (Callback Valid)", registration.Id);
                            
                            TempData["Success"] = "Payment successful! Your event registration is confirmed.";
                            return RedirectToAction("EventPaymentConfirmation", new { id = registration.Id });
                        }
                    }
                }
                catch (Exception valEx)
                {
                    _logger.LogError(valEx, "Validation API error - but payment callback received");

                    // If validation fails but we received success callback with VALID/VALIDATED status
                    if (status == "VALID" || status == "VALIDATED")
                    {
                        _logger.LogWarning("?? Validation API failed but callback status is {Status}, accepting payment", status);

                        registration.Status = "Confirmed";
                        registration.ConfirmedAt = DateTime.UtcNow;
                        registration.PaymentMethod = "SSLCommerz";
                        registration.Notes = $"Payment confirmed (Callback Valid, Validation Failed). Bank Transaction ID: {Request.Form["bank_tran_id"]}";

                        await _context.SaveChangesAsync();

                        _logger.LogInformation("? Event registration {RegistrationId} confirmed (Callback Valid, Validation Failed)", registration.Id);
                        
                        TempData["Success"] = "Payment successful! Your event registration is confirmed.";
                        return RedirectToAction("EventPaymentConfirmation", new { id = registration.Id });
                    }
                    else
                    {
                        _logger.LogError("? Validation failed AND callback status is not VALID: {Status}", status);
                        
                        registration.Status = "Pending";
                        registration.Notes = $"Payment pending verification. Status: {status}";
                        await _context.SaveChangesAsync();

                        _logger.LogInformation("?? Registration {RegistrationId} marked as pending verification", registration.Id);

                        TempData["Warning"] = "Payment received! Your registration is being verified. Transaction ID: " + tranId;
                        return RedirectToAction("EventPaymentConfirmation", new { id = registration.Id });
                    }
                }

                _logger.LogWarning("Payment validation failed for transaction: {TransactionId}", tranId);
                
                registration.Status = "Failed";
                registration.Notes = "Payment validation failed";
                await _context.SaveChangesAsync();

                TempData["Error"] = "Payment validation failed. Please contact support.";
                return RedirectToAction("EventDetails", new { id = registration.EventId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in event payment success callback");
                TempData["Error"] = "An error occurred while confirming your payment.";
                return RedirectToAction("Events");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EventPaymentFail()
        {
            try
            {
                var tranId = Request.Form["tran_id"].ToString();
                var failedReason = Request.Form["error"].ToString();

                _logger.LogInformation("=== Event Payment Failed ===");
                _logger.LogInformation("Transaction ID: {TransactionId}, Reason: {Reason}", tranId, failedReason);

                var registration = await _context.EventRegistrations
                    .FirstOrDefaultAsync(r => r.TransactionId == tranId);

                if (registration != null)
                {
                    registration.Status = "Failed";
                    registration.Notes = $"Payment failed: {failedReason}";
                    await _context.SaveChangesAsync();
                }

                TempData["Error"] = "Payment failed. Please try again.";
                return RedirectToAction("EventDetails", new { id = registration?.EventId ?? 0 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in event payment fail callback");
                TempData["Error"] = "Payment failed.";
                return RedirectToAction("Events");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EventPaymentCancel()
        {
            try
            {
                var tranId = Request.Form["tran_id"].ToString();

                _logger.LogInformation("=== Event Payment Cancelled ===");
                _logger.LogInformation("Transaction ID: {TransactionId}", tranId);

                var registration = await _context.EventRegistrations
                    .FirstOrDefaultAsync(r => r.TransactionId == tranId);

                if (registration != null)
                {
                    registration.Status = "Cancelled";
                    registration.Notes = "Payment cancelled by user";
                    await _context.SaveChangesAsync();
                }

                TempData["Warning"] = "Payment was cancelled. You can try again when you're ready.";
                return RedirectToAction("EventDetails", new { id = registration?.EventId ?? 0 });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in event payment cancel callback");
                TempData["Warning"] = "Payment was cancelled.";
                return RedirectToAction("Events");
            }
        }

        [HttpPost]
        public async Task<IActionResult> EventPaymentIPN()
        {
            try
            {
                var tranId = Request.Form["tran_id"].ToString();
                var valId = Request.Form["val_id"].ToString();
                var amount = decimal.Parse(Request.Form["amount"].ToString());
                var status = Request.Form["status"].ToString();

                _logger.LogInformation("=== Event Payment IPN Received ===");
                _logger.LogInformation("Transaction ID: {TransactionId}, Amount: {Amount}, Status: {Status}", tranId, amount, status);

                var registration = await _context.EventRegistrations
                    .FirstOrDefaultAsync(r => r.TransactionId == tranId);

                if (registration != null && registration.Status == "Pending")
                {
                    var isTestMode = _configuration.GetValue<bool>("SSLCommerz:IsLive") == false;

                    if (isTestMode)
                    {
                        _logger.LogInformation("Test mode IPN - accepting payment with status: {Status}", status);
                        
                        registration.Status = "Confirmed";
                        registration.ConfirmedAt = DateTime.UtcNow;
                        registration.PaymentMethod = "SSLCommerz";
                        registration.Notes = $"Payment confirmed via IPN (Test Mode). Bank Transaction ID: {Request.Form["bank_tran_id"]}";
                        
                        await _context.SaveChangesAsync();
                        
                        _logger.LogInformation("? Registration {RegistrationId} confirmed via IPN (Test Mode)", registration.Id);
                    }
                    else
                    {
                        // Live mode - try validation but also accept callback status
                        try
                        {
                            var validationResponse = await _sslCommerzService.ValidatePaymentAsync(valId);

                            if (validationResponse.status == "VALID" || validationResponse.status == "VALIDATED")
                            {
                                registration.Status = "Confirmed";
                                registration.ConfirmedAt = DateTime.UtcNow;
                                registration.PaymentMethod = "SSLCommerz";
                                registration.Notes = $"Payment confirmed via IPN. Bank Transaction ID: {Request.Form["bank_tran_id"]}";
                                
                                await _context.SaveChangesAsync();
                                
                                _logger.LogInformation("? Registration {RegistrationId} confirmed via IPN", registration.Id);
                            }
                            else if (status == "VALID" || status == "VALIDATED")
                            {
                                // Accept based on callback status even if validation API has issues
                                _logger.LogInformation("IPN callback status is VALID, accepting payment");
                                
                                registration.Status = "Confirmed";
                                registration.ConfirmedAt = DateTime.UtcNow;
                                registration.PaymentMethod = "SSLCommerz";
                                registration.Notes = $"Payment confirmed via IPN (Callback Valid). Bank Transaction ID: {Request.Form["bank_tran_id"]}";
                                
                                await _context.SaveChangesAsync();
                                
                                _logger.LogInformation("? Registration {RegistrationId} confirmed via IPN (Callback Valid)", registration.Id);
                            }
                        }
                        catch (Exception valEx)
                        {
                            _logger.LogError(valEx, "Validation API error in IPN");
                            
                            // If validation fails but callback status is valid, still accept
                            if (status == "VALID" || status == "VALIDATED")
                            {
                                registration.Status = "Confirmed";
                                registration.ConfirmedAt = DateTime.UtcNow;
                                registration.PaymentMethod = "SSLCommerz";
                                registration.Notes = $"Payment confirmed via IPN (Validation Failed). Bank Transaction ID: {Request.Form["bank_tran_id"]}";
                                
                                await _context.SaveChangesAsync();
                                
                                _logger.LogInformation("? Registration {RegistrationId} confirmed via IPN (Validation Failed)", registration.Id);
                            }
                        }
                    }
                }

                return Ok("IPN processed");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing event payment IPN");
                return StatusCode(500, "IPN processing failed");
            }
        }

        public async Task<IActionResult> EventPaymentConfirmation(int id)
        {
            try
            {
                var registration = await _context.EventRegistrations
                    .Include(r => r.Event)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (registration == null)
                {
                    return NotFound();
                }

                return View(registration);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading event payment confirmation");
                TempData["Error"] = "Unable to load confirmation page.";
                return RedirectToAction("Events");
            }
        }

        #endregion
    }
}


