using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Attributes;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.Services;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class NotificationTestController : Controller
    {
        private readonly ISmsService _smsService;
        private readonly IEmailService _emailService;
        private readonly ILogger<NotificationTestController> _logger;
        private readonly ApplicationDbContext _context;

        public NotificationTestController(
            ISmsService smsService,
            IEmailService emailService,
            ILogger<NotificationTestController> logger,
            ApplicationDbContext context)
        {
            _smsService = smsService;
            _emailService = emailService;
            _logger = logger;
            _context = context;
        }

        // GET: Admin/NotificationTest
        public IActionResult Index()
        {
            // Redirect to Templates page as the main page
            return RedirectToAction(nameof(Templates));
        }

        // GET: Admin/NotificationTest/TestNotifications
        public async Task<IActionResult> TestNotifications()
        {
            var donationTypes = await _context.DonationTypes
                .Where(d => d.IsActive)
                .OrderBy(d => d.DisplayOrder)
                .ToListAsync();
            
            ViewBag.DonationTypes = new SelectList(donationTypes, "Id", "Name");
            return View();
        }

        // GET: Admin/NotificationTest/Templates
        [Permission("Notification Templates", "View")]
        public async Task<IActionResult> Templates()
        {
            var templates = await _context.NotificationTemplates
                .Include(t => t.DonationType)
                .OrderByDescending(t => t.IsDefault)
                .ThenBy(t => t.Name)
                .ToListAsync();
            
            // Get SMS Balance
            var smsBalance = await _context.SmsBalances.FirstOrDefaultAsync();
            ViewBag.SmsBalance = smsBalance?.AvailableBalance ?? 0;
            ViewBag.LastUpdated = smsBalance?.LastUpdated;
            
            return View(templates);
        }

        // GET: Admin/NotificationTest/CreateTemplate
        [Permission("Notification Templates", "Create")]
        public async Task<IActionResult> CreateTemplate()
        {
            var donationTypes = await _context.DonationTypes
                .Where(d => d.IsActive)
                .OrderBy(d => d.DisplayOrder)
                .ToListAsync();
            
            ViewBag.DonationTypes = new SelectList(donationTypes, "Id", "Name");
            return View();
        }

        // POST: Admin/NotificationTest/CreateTemplate
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Notification Templates", "Create")]
        public async Task<IActionResult> CreateTemplate(NotificationTemplate template)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    template.CreatedAt = DateTime.UtcNow;
                    template.CreatedBy = User.Identity?.Name ?? "Admin";

                    _context.NotificationTemplates.Add(template);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Template created successfully!";
                    return RedirectToAction(nameof(Templates));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating template");
                TempData["Error"] = $"Error: {ex.Message}";
            }

            var donationTypes = await _context.DonationTypes
                .Where(d => d.IsActive)
                .OrderBy(d => d.DisplayOrder)
                .ToListAsync();
            
            ViewBag.DonationTypes = new SelectList(donationTypes, "Id", "Name");
            return View(template);
        }

        // GET: Admin/NotificationTest/EditTemplate/5
        [Permission("Notification Templates", "Edit")]
        public async Task<IActionResult> EditTemplate(int id)
        {
            var template = await _context.NotificationTemplates.FindAsync(id);
            if (template == null)
            {
                TempData["Error"] = "Template not found";
                return RedirectToAction(nameof(Templates));
            }

            var donationTypes = await _context.DonationTypes
                .Where(d => d.IsActive)
                .OrderBy(d => d.DisplayOrder)
                .ToListAsync();
            
            ViewBag.DonationTypes = new SelectList(donationTypes, "Id", "Name", template.DonationTypeId);
            return View(template);
        }

        // POST: Admin/NotificationTest/EditTemplate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Notification Templates", "Edit")]
        public async Task<IActionResult> EditTemplate(int id, NotificationTemplate template)
        {
            if (id != template.Id)
            {
                TempData["Error"] = "Invalid template ID";
                return RedirectToAction(nameof(Templates));
            }

            try
            {
                if (ModelState.IsValid)
                {
                    template.UpdatedAt = DateTime.UtcNow;
                    
                    _context.Update(template);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Template updated successfully!";
                    return RedirectToAction(nameof(Templates));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating template");
                TempData["Error"] = $"Error: {ex.Message}";
            }

            var donationTypes = await _context.DonationTypes
                .Where(d => d.IsActive)
                .OrderBy(d => d.DisplayOrder)
                .ToListAsync();
            
            ViewBag.DonationTypes = new SelectList(donationTypes, "Id", "Name", template.DonationTypeId);
            return View(template);
        }

        // POST: Admin/NotificationTest/DeleteTemplate/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Notification Templates", "Delete")]
        public async Task<IActionResult> DeleteTemplate(int id)
        {
            try
            {
                var template = await _context.NotificationTemplates.FindAsync(id);
                if (template == null)
                {
                    TempData["Error"] = "Template not found";
                    return RedirectToAction(nameof(Templates));
                }

                if (template.IsDefault)
                {
                    TempData["Error"] = "Cannot delete default template";
                    return RedirectToAction(nameof(Templates));
                }

                _context.NotificationTemplates.Remove(template);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Template deleted successfully!";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting template");
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(Templates));
        }

        // POST: Admin/NotificationTest/SendTestSms
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendTestSms(string phoneNumber, string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    TempData["Error"] = "Phone number is required";
                    return RedirectToAction(nameof(TestNotifications));
                }

                if (string.IsNullOrWhiteSpace(message))
                {
                    message = "This is a test SMS from WBS. If you received this, SMS service is working correctly!";
                }

                _logger.LogInformation("Testing SMS to {Phone}", phoneNumber);

                bool success = await _smsService.SendSmsAsync(phoneNumber, message);

                if (success)
                {
                    TempData["Success"] = $"Test SMS sent successfully to {phoneNumber}!";
                }
                else
                {
                    TempData["Warning"] = $"SMS sending may have failed. Check logs for details. (SMS might be disabled in settings)";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending test SMS");
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(TestNotifications));
        }

        // POST: Admin/NotificationTest/SendTestEmail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendTestEmail(string email, string subject, string message)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(email))
                {
                    TempData["Error"] = "Email is required";
                    return RedirectToAction(nameof(TestNotifications));
                }

                if (string.IsNullOrWhiteSpace(subject))
                {
                    subject = "Test Email from WBS";
                }

                if (string.IsNullOrWhiteSpace(message))
                {
                    message = "<h2>Test Email</h2><p>This is a test email from WBS. If you received this, email service is working correctly!</p>";
                }
                else
                {
                    if (!message.Contains("<html>") && !message.Contains("<p>"))
                    {
                        message = $"<html><body><p>{message.Replace("\n", "<br/>")}</p></body></html>";
                    }
                }

                _logger.LogInformation("Testing email to {Email}", email);

                bool success = await _emailService.SendEmailAsync(email, subject, message);

                if (success)
                {
                    TempData["Success"] = $"Test email sent successfully to {email}!";
                }
                else
                {
                    TempData["Warning"] = $"Email sending may have failed. Check logs for details. (Email might be disabled in settings)";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending test email");
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(TestNotifications));
        }

        // POST: Admin/NotificationTest/SendTestDonationReceipt
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendTestDonationReceipt(string phoneNumber, string email, string donorName, decimal amount, int? donationTypeId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(donorName))
                {
                    donorName = "Test Donor";
                }

                if (amount <= 0)
                {
                    amount = 1000;
                }

                // Get donation type name
                string donationTypeName = "General Donation";
                if (donationTypeId.HasValue)
                {
                    var donationType = await _context.DonationTypes.FindAsync(donationTypeId.Value);
                    if (donationType != null)
                    {
                        donationTypeName = donationType.Name;
                    }
                }

                string transactionId = $"WBS{DateTime.Now:yyyyMMddHHmmss}";
                
                // Get templates
                var smsTemplate = await _context.NotificationTemplates
                    .Where(t => t.TemplateType == "SMS" && t.IsActive)
                    .Where(t => t.DonationTypeId == donationTypeId || (t.DonationTypeId == null && t.IsDefault))
                    .OrderByDescending(t => t.DonationTypeId.HasValue)
                    .ThenByDescending(t => t.IsDefault)
                    .FirstOrDefaultAsync();

                var emailTemplate = await _context.NotificationTemplates
                    .Where(t => t.TemplateType == "Email" && t.IsActive)
                    .Where(t => t.DonationTypeId == donationTypeId || (t.DonationTypeId == null && t.IsDefault))
                    .OrderByDescending(t => t.DonationTypeId.HasValue)
                    .ThenByDescending(t => t.IsDefault)
                    .FirstOrDefaultAsync();

                var tasks = new List<Task<bool>>();

                // Send SMS
                if (!string.IsNullOrWhiteSpace(phoneNumber) && smsTemplate != null)
                {
                    string smsMessage = smsTemplate.SmsContent!
                        .Replace("{DonorName}", donorName)
                        .Replace("{Amount}", amount.ToString("N2"))
                        .Replace("{DonationType}", donationTypeName)
                        .Replace("{TransactionId}", transactionId);

                    _logger.LogInformation("Sending test donation receipt SMS to {Phone}", phoneNumber);
                    tasks.Add(_smsService.SendSmsAsync(phoneNumber, smsMessage));
                }

                // Send Email
                if (!string.IsNullOrWhiteSpace(email) && emailTemplate != null)
                {
                    string emailSubject = emailTemplate.EmailSubject!
                        .Replace("{TransactionId}", transactionId)
                        .Replace("{DonorName}", donorName)
                        .Replace("{Amount}", amount.ToString("N2"));

                    string emailMessage = emailTemplate.EmailContent!
                        .Replace("{DonorName}", donorName)
                        .Replace("{Amount}", amount.ToString("N2"))
                        .Replace("{DonationType}", donationTypeName)
                        .Replace("{TransactionId}", transactionId)
                        .Replace("{Date}", DateTime.Now.ToString("dd MMM yyyy, hh:mm tt"));

                    _logger.LogInformation("Sending test donation receipt email to {Email}", email);
                    tasks.Add(_emailService.SendEmailAsync(email, emailSubject, emailMessage));
                }

                if (tasks.Any())
                {
                    var results = await Task.WhenAll(tasks);
                    var successCount = results.Count(r => r);

                    if (successCount == tasks.Count)
                    {
                        TempData["Success"] = $"Test donation receipt sent successfully! ({successCount} notifications)";
                    }
                    else if (successCount > 0)
                    {
                        TempData["Warning"] = $"Partial success: {successCount} out of {tasks.Count} notifications sent";
                    }
                    else
                    {
                        TempData["Warning"] = "Notifications may have failed. Check logs for details.";
                    }
                }
                else
                {
                    TempData["Error"] = "Please provide at least one contact method (phone or email) and ensure templates are configured";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending test donation receipt");
                TempData["Error"] = $"Error: {ex.Message}";
            }

            return RedirectToAction(nameof(TestNotifications));
        }

        // GET: Admin/NotificationTest/SmsLogs
        public async Task<IActionResult> SmsLogs()
        {
            var logs = await _context.SmsLogs
                .OrderByDescending(l => l.SentAt)
                .Take(100)
                .ToListAsync();
            
            // Get SMS Balance
            var smsBalance = await _context.SmsBalances.FirstOrDefaultAsync();
            ViewBag.SmsBalance = smsBalance?.AvailableBalance ?? 0;
            
            return View(logs);
        }
    }
}
