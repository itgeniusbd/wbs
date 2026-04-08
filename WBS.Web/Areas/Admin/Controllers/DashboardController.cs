using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Services;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize] // All authenticated users can access dashboard
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IDonationService _donationService;

        public DashboardController(ApplicationDbContext context, IDonationService donationService)
        {
            _context = context;
            _donationService = donationService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                ViewBag.TotalDonations = await _donationService.GetTotalDonationsAsync();
                ViewBag.DonationsCount = await _donationService.GetDonationsCountAsync();
                ViewBag.VolunteersCount = await _context.Volunteers.CountAsync();
                ViewBag.MessagesCount = await _context.ContactMessages.CountAsync(m => !m.IsRead);
                ViewBag.AppealsCount = await _context.Appeals.CountAsync(a => a.IsActive);
                ViewBag.NewsCount = await _context.News.CountAsync();

                var recentDonations = await _context.Donations
                    .AsNoTracking()
                    .Include(d => d.DonationType)
                    .Where(d => d.DonationType != null 
                             && d.DonorName != null 
                             && d.Email != null 
                             && d.Phone != null)
                    .OrderByDescending(d => d.CreatedAt)
                    .Take(10)
                    .ToListAsync();

                return View(recentDonations);
            }
            catch (Exception ex)
            {
                // Log the error and show a friendly message
                ViewBag.ErrorMessage = "Error loading dashboard data. Please check the database for NULL values in required fields.";
                ViewBag.TechnicalError = ex.Message;
                return View(new List<WBS.Web.Models.Donation>());
            }
        }
    }
}
