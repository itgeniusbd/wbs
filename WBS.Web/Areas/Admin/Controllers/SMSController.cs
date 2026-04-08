using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.Services;
using WBS.Web.Attributes;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SMSController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SMSController> _logger;
        private readonly ISmsService _smsService;

        public SMSController(
            ApplicationDbContext context,
            ILogger<SMSController> logger,
            ISmsService smsService)
        {
            _context = context;
            _logger = logger;
            _smsService = smsService;
        }

        // GET: Admin/SMS/SendSMS
        [Permission("SMS Management", "Send")]
        public async Task<IActionResult> SendSMS()
        {
            try
            {
                // Get SMS Balance
                var smsBalance = await _smsService.GetSmsBalanceAsync();
                ViewBag.SmsBalance = smsBalance;

                // Load contact groups for dropdown with contacts included
                ViewBag.ContactGroups = await _context.ContactGroups
                    .Include(g => g.Contacts)
                    .Where(g => g.IsActive)
                    .OrderBy(g => g.GroupName)
                    .ToListAsync();

                // Get counts for display
                ViewBag.TotalDonors = await _context.Donations
                    .Where(d => !string.IsNullOrEmpty(d.Phone))
                    .Select(d => d.Phone)
                    .Distinct()
                    .CountAsync();

                ViewBag.TotalVolunteers = await _context.Volunteers
                    .Where(v => v.Status == "Approved" && !string.IsNullOrEmpty(v.Phone))
                    .CountAsync();

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading Send SMS page");
                TempData["Error"] = "Error loading page.";
                return View();
            }
        }

        // POST: Admin/SMS/SendSMS
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("SMS Management", "Send")]
        public async Task<IActionResult> SendSMS(string recipientType, string message, string? selectedIds, int? contactGroupId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(message))
                {
                    TempData["Error"] = "Please enter a message.";
                    return RedirectToAction(nameof(SendSMS));
                }

                // Create SMS Campaign
                var campaign = new SMSCampaign
                {
                    CampaignName = $"{recipientType} - {DateTime.Now:dd MMM yyyy HH:mm}",
                    Message = message,
                    RecipientType = recipientType,
                    ContactGroupId = contactGroupId,
                    CreatedBy = User.Identity?.Name,
                    CreatedAt = DateTime.UtcNow,
                    Status = "Sending"
                };

                // Get recipients based on type
                var recipients = new List<SMSCampaignRecipient>();

                switch (recipientType)
                {
                    case "AllDonors":
                        var allDonors = await _context.Donations
                            .Where(d => d.Status == DonationStatus.Completed && !string.IsNullOrEmpty(d.Phone))
                            .Select(d => new { d.DonorName, d.Phone })
                            .Distinct()
                            .ToListAsync();

                        recipients = allDonors.Select(d => new SMSCampaignRecipient
                        {
                            Name = d.DonorName,
                            PhoneNumber = d.Phone,
                            Status = "Pending"
                        }).ToList();
                        break;

                    case "SelectedDonors":
                        if (!string.IsNullOrEmpty(selectedIds))
                        {
                            var ids = selectedIds.Split(',').Select(int.Parse).ToList();
                            var selectedDonors = await _context.Donations
                                .Where(d => ids.Contains(d.Id) && !string.IsNullOrEmpty(d.Phone))
                                .Select(d => new { d.DonorName, d.Phone })
                                .Distinct()
                                .ToListAsync();

                            recipients = selectedDonors.Select(d => new SMSCampaignRecipient
                            {
                                Name = d.DonorName,
                                PhoneNumber = d.Phone,
                                Status = "Pending"
                            }).ToList();
                        }
                        break;

                    case "AllVolunteers":
                        var allVolunteers = await _context.Volunteers
                            .Where(v => v.Status == "Approved" && !string.IsNullOrEmpty(v.Phone))
                            .ToListAsync();

                        recipients = allVolunteers.Select(v => new SMSCampaignRecipient
                        {
                            Name = $"{v.FirstName} {v.LastName}",
                            PhoneNumber = v.Phone!,
                            Status = "Pending"
                        }).ToList();
                        break;

                    case "SelectedVolunteers":
                        if (!string.IsNullOrEmpty(selectedIds))
                        {
                            var ids = selectedIds.Split(',').Select(int.Parse).ToList();
                            var selectedVolunteers = await _context.Volunteers
                                .Where(v => ids.Contains(v.Id) && !string.IsNullOrEmpty(v.Phone))
                                .ToListAsync();

                            recipients = selectedVolunteers.Select(v => new SMSCampaignRecipient
                            {
                                Name = $"{v.FirstName} {v.LastName}",
                                PhoneNumber = v.Phone!,
                                Status = "Pending"
                            }).ToList();
                        }
                        break;

                    case "ContactGroup":
                        if (contactGroupId.HasValue)
                        {
                            var contactList = await _context.ContactListItems
                                .Where(c => c.ContactGroupId == contactGroupId && c.IsActive)
                                .ToListAsync();

                            recipients = contactList.Select(c => new SMSCampaignRecipient
                            {
                                Name = c.Name,
                                PhoneNumber = c.PhoneNumber,
                                Status = "Pending"
                            }).ToList();
                        }
                        break;
                }

                if (!recipients.Any())
                {
                    TempData["Error"] = "No recipients found.";
                    return RedirectToAction(nameof(SendSMS));
                }

                campaign.TotalRecipients = recipients.Count;
                campaign.Recipients = recipients;

                _context.SMSCampaigns.Add(campaign);
                await _context.SaveChangesAsync();

                // Send SMS immediately (not background task)
                await SendSMSBulkAsync(campaign.Id);

                TempData["Success"] = $"SMS campaign created! Check status in SMS Sent Records.";
                return RedirectToAction(nameof(SMSSentRecords));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending SMS");
                TempData["Error"] = "Error sending SMS.";
                return RedirectToAction(nameof(SendSMS));
            }
        }

        // Background task to send SMS
        private async Task SendSMSBulkAsync(int campaignId)
        {
            try
            {
                var campaign = await _context.SMSCampaigns
                    .Include(c => c.Recipients)
                    .FirstOrDefaultAsync(c => c.Id == campaignId);

                if (campaign == null) return;

                int successCount = 0;
                int failedCount = 0;

                foreach (var recipient in campaign.Recipients)
                {
                    try
                    {
                        var result = await _smsService.SendSmsAsync(recipient.PhoneNumber, campaign.Message);

                        if (result)
                        {
                            recipient.Status = "Sent";
                            recipient.SentAt = DateTime.UtcNow;
                            successCount++;
                        }
                        else
                        {
                            recipient.Status = "Failed";
                            recipient.ErrorMessage = "SMS service returned false";
                            failedCount++;
                        }
                    }
                    catch (Exception ex)
                    {
                        recipient.Status = "Failed";
                        recipient.ErrorMessage = ex.Message;
                        failedCount++;
                        _logger.LogError(ex, $"Error sending SMS to {recipient.PhoneNumber}");
                    }

                    // Small delay between messages
                    await Task.Delay(100);
                }

                campaign.SuccessCount = successCount;
                campaign.FailedCount = failedCount;
                campaign.Status = "Completed";
                campaign.SentAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in bulk SMS sending for campaign {campaignId}");
            }
        }

        // GET: Admin/SMS/SMSSentRecords
        [Permission("SMS Management", "View")]
        public async Task<IActionResult> SMSSentRecords(DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var query = _context.SMSCampaigns
                    .Include(c => c.ContactGroup)
                    .AsQueryable();

                if (fromDate.HasValue)
                    query = query.Where(c => c.CreatedAt >= fromDate.Value);

                if (toDate.HasValue)
                    query = query.Where(c => c.CreatedAt <= toDate.Value);

                var campaigns = await query
                    .OrderByDescending(c => c.CreatedAt)
                    .ToListAsync();

                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;

                return View(campaigns);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading SMS sent records");
                TempData["Error"] = "Error loading records.";
                return View(new List<SMSCampaign>());
            }
        }

        // GET: Admin/SMS/CampaignDetails/5
        [Permission("SMS Management", "View")]
        public async Task<IActionResult> CampaignDetails(int id)
        {
            try
            {
                var campaign = await _context.SMSCampaigns
                    .Include(c => c.ContactGroup)
                    .Include(c => c.Recipients)
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (campaign == null)
                {
                    TempData["Error"] = "Campaign not found.";
                    return RedirectToAction(nameof(SMSSentRecords));
                }

                return View(campaign);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading campaign details");
                TempData["Error"] = "Error loading campaign details.";
                return RedirectToAction(nameof(SMSSentRecords));
            }
        }

        // POST: Admin/SMS/RetryCampaign
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("SMS Management", "Send")]
        public async Task<IActionResult> RetryCampaign(int campaignId)
        {
            try
            {
                var campaign = await _context.SMSCampaigns
                    .Include(c => c.Recipients)
                    .FirstOrDefaultAsync(c => c.Id == campaignId);

                if (campaign == null)
                {
                    TempData["Error"] = "Campaign not found.";
                    return RedirectToAction(nameof(SMSSentRecords));
                }

                // Reset status for failed/pending recipients
                foreach (var recipient in campaign.Recipients.Where(r => r.Status != "Sent"))
                {
                    recipient.Status = "Pending";
                    recipient.ErrorMessage = null;
                    recipient.SentAt = null;
                }

                campaign.Status = "Sending";
                await _context.SaveChangesAsync();

                // Retry sending
                await SendSMSBulkAsync(campaignId);

                TempData["Success"] = "SMS campaign retried successfully.";
                return RedirectToAction(nameof(CampaignDetails), new { id = campaignId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrying campaign");
                TempData["Error"] = "Error retrying campaign.";
                return RedirectToAction(nameof(SMSSentRecords));
            }
        }

        // GET: Admin/SMS/GetDonors (API)
        [HttpGet]
        public async Task<IActionResult> GetDonors()
        {
            try
            {
                var donors = await _context.Donations
                    .Include(d => d.DonationType)
                    .Where(d => d.Status == DonationStatus.Completed && !string.IsNullOrEmpty(d.Phone))
                    .GroupBy(d => new { d.DonorName, d.Phone })
                    .Select(g => new
                    {
                        id = g.First().Id,
                        name = g.Key.DonorName,
                        phone = g.Key.Phone,
                        donorType = g.First().DonationType != null ? g.First().DonationType.Name : "General",
                        totalDonations = g.Count(),
                        totalAmount = g.Sum(d => d.Amount)
                    })
                    .OrderByDescending(d => d.totalAmount)
                    .ToListAsync();

                return Json(donors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting donors");
                return Json(new List<object>());
            }
        }

        // GET: Admin/SMS/GetVolunteers (API)
        [HttpGet]
        public async Task<IActionResult> GetVolunteers()
        {
            try
            {
                var volunteers = await _context.Volunteers
                    .Where(v => v.Status == "Approved" && !string.IsNullOrEmpty(v.Phone))
                    .Select(v => new
                    {
                        id = v.Id,
                        name = v.FirstName + " " + v.LastName,
                        phone = v.Phone,
                        email = v.Email
                    })
                    .ToListAsync();

                return Json(volunteers);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting volunteers");
                return Json(new List<object>());
            }
        }
    }
}
