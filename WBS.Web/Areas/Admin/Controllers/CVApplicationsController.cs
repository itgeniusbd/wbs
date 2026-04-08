using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CVApplicationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CVApplicationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string status = "All", int page = 1, int pageSize = 20)
        {
            var query = _context.CVApplications.AsQueryable();

            if (status != "All")
            {
                query = query.Where(c => c.Status == status);
            }

            ViewBag.CurrentStatus = status;
            ViewBag.TotalCount = await query.CountAsync();
            ViewBag.PendingCount = await _context.CVApplications.CountAsync(c => c.Status == "Pending");
            ViewBag.ReviewedCount = await _context.CVApplications.CountAsync(c => c.Status == "Reviewed");
            ViewBag.ShortlistedCount = await _context.CVApplications.CountAsync(c => c.Status == "Shortlisted");
            ViewBag.RejectedCount = await _context.CVApplications.CountAsync(c => c.Status == "Rejected");

            var applications = await query
                .OrderByDescending(c => c.AppliedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)ViewBag.TotalCount / pageSize);

            return View(applications);
        }

        public async Task<IActionResult> Details(int id)
        {
            var application = await _context.CVApplications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            return View(application);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id, string status, string? reviewNotes)
        {
            var application = await _context.CVApplications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            application.Status = status;
            application.ReviewedDate = DateTime.UtcNow;
            if (!string.IsNullOrEmpty(reviewNotes))
            {
                application.ReviewNotes = reviewNotes;
            }

            await _context.SaveChangesAsync();

            TempData["Success"] = "Application status updated successfully!";
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var application = await _context.CVApplications.FindAsync(id);
            if (application == null)
            {
                return NotFound();
            }

            // Delete CV file if exists
            if (!string.IsNullOrEmpty(application.CVFilePath))
            {
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", application.CVFilePath.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            _context.CVApplications.Remove(application);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Application deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
