using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.Services;
using WBS.Web.Attributes;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class DonationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DonationsController> _logger;
        private readonly ISmsService _smsService;
        private readonly IEmailService _emailService;

        public DonationsController(
            ApplicationDbContext context, 
            ILogger<DonationsController> logger,
            ISmsService smsService,
            IEmailService emailService)
        {
            _context = context;
            _logger = logger;
            _smsService = smsService;
            _emailService = emailService;
        }

        [Permission("Donations", "View")]
        public async Task<IActionResult> Index(string searchTerm, int? donationTypeId, DonationStatus? status, DateTime? fromDate, DateTime? toDate, int page = 1)
        {
            var pageSize = 20;
            var query = _context.Donations
                .Include(d => d.DonationType)
                .Include(d => d.Appeal)
                .AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(d => 
                    d.DonorName.Contains(searchTerm) || 
                    (d.Email != null && d.Email.Contains(searchTerm)) || 
                    (d.Phone != null && d.Phone.Contains(searchTerm)) ||
                    (d.TransactionId != null && d.TransactionId.Contains(searchTerm)));
            }

            // Filter by donation type
            if (donationTypeId.HasValue)
            {
                query = query.Where(d => d.DonationTypeId == donationTypeId.Value);
            }

            // Filter by status
            if (status.HasValue)
            {
                query = query.Where(d => d.Status == status.Value);
            }

            // Filter by date range
            if (fromDate.HasValue)
            {
                query = query.Where(d => d.CreatedAt >= fromDate.Value);
            }

            if (toDate.HasValue)
            {
                var endDate = toDate.Value.AddDays(1);
                query = query.Where(d => d.CreatedAt < endDate);
            }

            var totalCount = await query.CountAsync();
            var donations = await query
                .OrderByDescending(d => d.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Calculate statistics
            ViewBag.TotalDonations = totalCount;
            ViewBag.TotalAmount = await query.SumAsync(d => d.Amount);
            ViewBag.PendingAmount = await query.Where(d => d.Status == DonationStatus.Pending).SumAsync(d => d.Amount);
            ViewBag.CompletedAmount = await query.Where(d => d.Status == DonationStatus.Completed).SumAsync(d => d.Amount);

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            // Load filter options
            ViewBag.DonationTypes = await _context.DonationTypes
                .Where(dt => dt.IsActive)
                .Select(dt => new SelectListItem { Value = dt.Id.ToString(), Text = dt.Name })
                .ToListAsync();

            ViewBag.SearchTerm = searchTerm;
            ViewBag.SelectedTypeId = donationTypeId;
            ViewBag.SelectedStatus = status;
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            return View(donations);
        }

        // GET: Admin/Donations/Create - Manual Entry
        [Permission("Donations", "Create")]
        public async Task<IActionResult> Create()
        {
            ViewBag.DonationTypes = await _context.DonationTypes
                .Where(dt => dt.IsActive)
                .Select(dt => new SelectListItem { Value = dt.Id.ToString(), Text = dt.Name })
                .ToListAsync();

            ViewBag.Appeals = await _context.Appeals
                .Where(a => a.IsActive)
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Title })
                .ToListAsync();

            ViewBag.SDGs = await _context.SDGs
                .Where(s => s.IsActive)
                .OrderBy(s => s.Number)
                .Select(s => new SelectListItem { 
                    Value = s.Id.ToString(), 
                    Text = $"SDG {s.Number} - {s.Name}" 
                })
                .ToListAsync();

            ViewBag.Accounts = await _context.Accounts
                .Where(a => a.IsActive)
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.AccountName })
                .ToListAsync();

            return View();
        }

        // POST: Admin/Donations/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Donations", "Create")]
        public async Task<IActionResult> Create(Donation donation, bool sendNotifications = true)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    donation.CreatedAt = DateTime.UtcNow;
                    
                    // Set PaidAt if status is Completed
                    if (donation.Status == DonationStatus.Completed && !donation.PaidAt.HasValue)
                    {
                        donation.PaidAt = DateTime.UtcNow;
                    }
                    
                    // Set PaymentDate if status is Completed
                    if (donation.Status == DonationStatus.Completed && !donation.PaymentDate.HasValue)
                    {
                        donation.PaymentDate = DateTime.UtcNow;
                    }
                    
                    if (string.IsNullOrEmpty(donation.TransactionId))
                    {
                        donation.TransactionId = GenerateTransactionId();
                    }
                    
                    _context.Donations.Add(donation);
                    await _context.SaveChangesAsync();

                    // Update appeal raised amount if completed
                    if (donation.AppealId.HasValue && donation.Status == DonationStatus.Completed)
                    {
                        var appeal = await _context.Appeals.FindAsync(donation.AppealId.Value);
                        if (appeal != null)
                        {
                            appeal.RaisedAmount += donation.Amount;
                            await _context.SaveChangesAsync();
                        }
                    }

                    if (sendNotifications)
                    {
                        await SendDonationNotificationsAsync(donation);
                    }

                    TempData["Success"] = "Donation created successfully!";
                    return RedirectToAction(nameof(Details), new { id = donation.Id });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error creating donation");
                    TempData["Error"] = "Failed to create donation: " + ex.Message;
                }
            }

            ViewBag.DonationTypes = await _context.DonationTypes
                .Where(dt => dt.IsActive)
                .Select(dt => new SelectListItem { Value = dt.Id.ToString(), Text = dt.Name })
                .ToListAsync();

            ViewBag.Appeals = await _context.Appeals
                .Where(a => a.IsActive)
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Title })
                .ToListAsync();

            ViewBag.SDGs = await _context.SDGs
                .Where(s => s.IsActive)
                .OrderBy(s => s.Number)
                .Select(s => new SelectListItem { 
                    Value = s.Id.ToString(), 
                    Text = $"SDG {s.Number} - {s.Name}" 
                })
                .ToListAsync();

            ViewBag.Accounts = await _context.Accounts
                .Where(a => a.IsActive)
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.AccountName })
                .ToListAsync();

            return View(donation);
        }

        [Permission("Donations", "View")]
        public async Task<IActionResult> Details(int id)
        {
            var donation = await _context.Donations
                .Include(d => d.DonationType)
                .Include(d => d.Appeal)
                .FirstOrDefaultAsync(d => d.Id == id);

            if (donation == null)
                return NotFound();

            return View(donation);
        }

        // GET: Admin/Donations/Edit/5
        [Permission("Donations", "Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
                return NotFound();

            ViewBag.DonationTypes = await _context.DonationTypes
                .Where(dt => dt.IsActive)
                .Select(dt => new SelectListItem { Value = dt.Id.ToString(), Text = dt.Name })
                .ToListAsync();

            ViewBag.Appeals = await _context.Appeals
                .Where(a => a.IsActive)
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Title })
                .ToListAsync();

            ViewBag.SDGs = await _context.SDGs
                .Where(s => s.IsActive)
                .OrderBy(s => s.Number)
                .Select(s => new SelectListItem { 
                    Value = s.Id.ToString(), 
                    Text = $"SDG {s.Number} - {s.Name}" 
                })
                .ToListAsync();

            ViewBag.Accounts = await _context.Accounts
                .Where(a => a.IsActive)
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.AccountName })
                .ToListAsync();

            return View(donation);
        }

        // POST: Admin/Donations/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Donations", "Edit")]
        public async Task<IActionResult> Edit(int id, Donation donation)
        {
            if (id != donation.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existingDonation = await _context.Donations.AsNoTracking().FirstOrDefaultAsync(d => d.Id == id);
                    if (existingDonation == null)
                        return NotFound();

                    // Preserve CreatedAt from existing donation
                    donation.CreatedAt = existingDonation.CreatedAt;

                    // Handle status change from non-Completed to Completed
                    if (existingDonation.Status != DonationStatus.Completed && donation.Status == DonationStatus.Completed)
                    {
                        // Set payment dates when marking as completed
                        if (!donation.PaidAt.HasValue)
                        {
                            donation.PaidAt = DateTime.UtcNow;
                        }
                        if (!donation.PaymentDate.HasValue)
                        {
                            donation.PaymentDate = DateTime.UtcNow;
                        }
                        if (string.IsNullOrEmpty(donation.PaymentStatus))
                        {
                            donation.PaymentStatus = "Paid";
                        }
                    }
                    else if (donation.Status == DonationStatus.Completed)
                    {
                        // Preserve existing payment dates if already completed
                        donation.PaidAt = existingDonation.PaidAt ?? DateTime.UtcNow;
                        donation.PaymentDate = existingDonation.PaymentDate ?? DateTime.UtcNow;
                        donation.PaymentStatus = existingDonation.PaymentStatus ?? "Paid";
                    }
                    else
                    {
                        // If not completed, clear payment dates
                        donation.PaidAt = null;
                        donation.PaymentDate = null;
                        donation.PaymentStatus = existingDonation.PaymentStatus;
                    }

                    // Preserve payment tracking fields
                    donation.BankTransactionId = existingDonation.BankTransactionId;
                    donation.CardType = existingDonation.CardType;

                    // Update account balance if AccountId changed and status is Completed
                    if (donation.Status == DonationStatus.Completed && donation.AccountId.HasValue)
                    {
                        // If AccountId changed, update both accounts
                        if (existingDonation.AccountId != donation.AccountId)
                        {
                            // Remove from old account if it existed
                            if (existingDonation.AccountId.HasValue && existingDonation.Status == DonationStatus.Completed)
                            {
                                var oldAccount = await _context.Accounts.FindAsync(existingDonation.AccountId.Value);
                                if (oldAccount != null)
                                {
                                    oldAccount.AccountBalance -= existingDonation.Amount;
                                    oldAccount.Total_Income -= existingDonation.Amount;
                                    oldAccount.Total_IN -= existingDonation.Amount;
                                    oldAccount.UpdatedAt = DateTime.UtcNow;
                                }
                            }

                            // Add to new account
                            var newAccount = await _context.Accounts.FindAsync(donation.AccountId.Value);
                            if (newAccount != null)
                            {
                                newAccount.AccountBalance += donation.Amount;
                                newAccount.Total_Income += donation.Amount;
                                newAccount.Total_IN += donation.Amount;
                                newAccount.UpdatedAt = DateTime.UtcNow;
                            }
                        }
                        // If amount changed but same account
                        else if (existingDonation.Amount != donation.Amount && existingDonation.Status == DonationStatus.Completed)
                        {
                            var account = await _context.Accounts.FindAsync(donation.AccountId.Value);
                            if (account != null)
                            {
                                var difference = donation.Amount - existingDonation.Amount;
                                account.AccountBalance += difference;
                                account.Total_Income += difference;
                                account.Total_IN += difference;
                                account.UpdatedAt = DateTime.UtcNow;
                            }
                        }
                        // If status changed from non-Completed to Completed
                        else if (existingDonation.Status != DonationStatus.Completed)
                        {
                            var account = await _context.Accounts.FindAsync(donation.AccountId.Value);
                            if (account != null)
                            {
                                account.AccountBalance += donation.Amount;
                                account.Total_Income += donation.Amount;
                                account.Total_IN += donation.Amount;
                                account.UpdatedAt = DateTime.UtcNow;
                            }
                        }
                    }
                    // If status changed from Completed to non-Completed, remove from account
                    else if (existingDonation.Status == DonationStatus.Completed && donation.Status != DonationStatus.Completed)
                    {
                        if (existingDonation.AccountId.HasValue)
                        {
                            var account = await _context.Accounts.FindAsync(existingDonation.AccountId.Value);
                            if (account != null)
                            {
                                account.AccountBalance -= existingDonation.Amount;
                                account.Total_Income -= existingDonation.Amount;
                                account.Total_IN -= existingDonation.Amount;
                                account.UpdatedAt = DateTime.UtcNow;
                            }
                        }
                    }

                    // Update appeal raised amount if needed
                    if (existingDonation.AppealId != donation.AppealId || existingDonation.Amount != donation.Amount || existingDonation.Status != donation.Status)
                    {
                        // Remove from old appeal if it was completed
                        if (existingDonation.AppealId.HasValue && existingDonation.Status == DonationStatus.Completed)
                        {
                            var oldAppeal = await _context.Appeals.FindAsync(existingDonation.AppealId.Value);
                            if (oldAppeal != null)
                            {
                                oldAppeal.RaisedAmount -= existingDonation.Amount;
                            }
                        }

                        // Add to new appeal if completed
                        if (donation.AppealId.HasValue && donation.Status == DonationStatus.Completed)
                        {
                            var newAppeal = await _context.Appeals.FindAsync(donation.AppealId.Value);
                            if (newAppeal != null)
                            {
                                newAppeal.RaisedAmount += donation.Amount;
                            }
                        }
                    }

                    _context.Update(donation);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Donation updated successfully!";
                    return RedirectToAction(nameof(Details), new { id = donation.Id });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error updating donation");
                    TempData["Error"] = "Failed to update donation: " + ex.Message;
                }
            }

            ViewBag.DonationTypes = await _context.DonationTypes
                .Where(dt => dt.IsActive)
                .Select(dt => new SelectListItem { Value = dt.Id.ToString(), Text = dt.Name })
                .ToListAsync();

            ViewBag.Appeals = await _context.Appeals
                .Where(a => a.IsActive)
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.Title })
                .ToListAsync();

            ViewBag.SDGs = await _context.SDGs
                .Where(s => s.IsActive)
                .OrderBy(s => s.Number)
                .Select(s => new SelectListItem { 
                    Value = s.Id.ToString(), 
                    Text = $"SDG {s.Number} - {s.Name}" 
                })
                .ToListAsync();

            ViewBag.Accounts = await _context.Accounts
                .Where(a => a.IsActive)
                .Select(a => new SelectListItem { Value = a.Id.ToString(), Text = a.AccountName })
                .ToListAsync();

            return View(donation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Donations", "Edit")]
        public async Task<IActionResult> UpdateStatus(int id, DonationStatus status)
        {
            var donation = await _context.Donations.FindAsync(id);
            if (donation == null)
                return NotFound();

            var oldStatus = donation.Status;
            donation.Status = status;
            
            if (status == DonationStatus.Completed)
            {
                donation.PaidAt = DateTime.UtcNow;

                // Update appeal raised amount only if status changed
                if (oldStatus != DonationStatus.Completed && donation.AppealId.HasValue)
                {
                    var appeal = await _context.Appeals.FindAsync(donation.AppealId.Value);
                    if (appeal != null)
                    {
                        appeal.RaisedAmount += donation.Amount;
                    }
                }
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Donation status updated!";
            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: Admin/Donations/Invoice/5
        [Permission("Donations", "View")]
        public async Task<IActionResult> Invoice(int? id)
        {
            if (id == null)
                return NotFound();

            var donation = await _context.Donations
                .Include(d => d.DonationType)
                .Include(d => d.Appeal)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (donation == null)
                return NotFound();

            var siteSettings = await _context.SiteSettings.FirstOrDefaultAsync();
            ViewBag.SiteSettings = siteSettings;

            return View(donation);
        }

        // GET: Admin/Donations/Reports
        [Permission("Donations", "View")]
        public async Task<IActionResult> Reports(DateTime? fromDate, DateTime? toDate, string donorType = null)
        {
            var query = _context.Donations.AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(d => d.CreatedAt >= fromDate.Value);

            if (toDate.HasValue)
            {
                var endDate = toDate.Value.AddDays(1);
                query = query.Where(d => d.CreatedAt < endDate);
            }

            if (!string.IsNullOrEmpty(donorType))
                query = query.Where(d => d.DonorType == donorType);

            // Group by donation type
            var donationsByType = await _context.Donations
                .Include(d => d.DonationType)
                .Where(d => d.Status == DonationStatus.Completed)
                .GroupBy(d => new { d.DonationTypeId, Name = d.DonationType != null ? d.DonationType.Name : string.Empty })
                .Select(g => new
                {
                    Type = g.Key.Name,
                    Count = g.Count(),
                    Amount = g.Sum(d => d.Amount)
                })
                .ToListAsync();

            // Group by donor type
            var donationsByDonorType = await _context.Donations
                .Where(d => d.Status == DonationStatus.Completed && !string.IsNullOrEmpty(d.DonorType))
                .GroupBy(d => d.DonorType)
                .Select(g => new
                {
                    Type = g.Key ?? "Not Specified",
                    Count = g.Count(),
                    Amount = g.Sum(d => d.Amount)
                })
                .ToListAsync();

            ViewBag.DonationsByType = donationsByType;
            ViewBag.DonationsByDonorType = donationsByDonorType;
            ViewBag.TotalDonations = await query.CountAsync();
            ViewBag.TotalAmount = await query.SumAsync(d => d.Amount);
            ViewBag.CompletedDonations = await query.Where(d => d.Status == DonationStatus.Completed).CountAsync();
            ViewBag.CompletedAmount = await query.Where(d => d.Status == DonationStatus.Completed).SumAsync(d => d.Amount);
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");
            ViewBag.SelectedDonorType = donorType;

            return View();
        }

        // POST: Admin/Donations/SendNotification/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Donations", "Edit")]
        public async Task<IActionResult> SendNotification(int id)
        {
            try
            {
                var donation = await _context.Donations
                    .Include(d => d.DonationType)
                    .FirstOrDefaultAsync(d => d.Id == id);

                if (donation == null)
                    return NotFound();

                await SendDonationNotificationsAsync(donation);

                TempData["Success"] = "Notification sent successfully!";
                return RedirectToAction(nameof(Details), new { id });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification");
                TempData["Error"] = "Failed to send notification";
                return RedirectToAction(nameof(Details), new { id });
            }
        }

        [Permission("Donations", "View")]
        public async Task<IActionResult> Export()
        {
            var donations = await _context.Donations
                .Include(d => d.DonationType)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();

            // Generate CSV
            var csv = "Id,Donor Name,Email,Phone,Amount,Currency,Type,Status,Payment Method,Date\n";
            foreach (var d in donations)
            {
                csv += $"{d.Id},{d.DonorName},{d.Email},{d.Phone ?? ""},{d.Amount},{d.Currency},{d.DonationType?.Name ?? ""},{d.Status},{d.PaymentMethod},{d.CreatedAt:yyyy-MM-dd HH:mm}\n";
            }

            return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", $"donations_{DateTime.Now:yyyyMMdd}.csv");
        }

        #region Helper Methods

        private string GenerateTransactionId()
        {
            return $"WBS{DateTime.UtcNow:yyyyMMddHHmmss}{new Random().Next(1000, 9999)}";
        }

        private async Task SendDonationNotificationsAsync(Donation donation)
        {
            var tasks = new List<Task<bool>>();

            // Send Email
            if (!string.IsNullOrEmpty(donation.Email))
            {
                _logger.LogInformation("Sending email to {Email} for donation {DonationId}", donation.Email, donation.Id);
                tasks.Add(_emailService.SendDonationReceiptAsync(
                    donation.Email, 
                    donation.DonorName, 
                    donation.Amount, 
                    donation.TransactionId ?? "N/A",
                    donation.Id));
            }

            // Send SMS
            if (!string.IsNullOrEmpty(donation.Phone))
            {
                _logger.LogInformation("Sending SMS to {Phone} for donation {DonationId}", donation.Phone, donation.Id);
                tasks.Add(_smsService.SendDonationReceiptAsync(
                    donation.Phone, 
                    donation.DonorName, 
                    donation.Amount, 
                    donation.TransactionId ?? "N/A"));
            }

            // Wait for all notifications to complete
            if (tasks.Any())
            {
                var results = await Task.WhenAll(tasks);
                var successCount = results.Count(r => r);
                _logger.LogInformation("Sent {SuccessCount} out of {TotalCount} notifications for donation {DonationId}", 
                    successCount, tasks.Count, donation.Id);
            }
        }

        #endregion
    }
}
