using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Attributes;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class VolunteersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VolunteersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Permission("Volunteers", "View")]
        public async Task<IActionResult> Index(int? projectId, DateTime? fromDate, DateTime? toDate, string? status)
        {
            var query = _context.Volunteers
                .Include(v => v.SDGProject)
                .AsQueryable();

            // Filter by project
            if (projectId.HasValue && projectId.Value > 0)
            {
                query = query.Where(v => v.SDGProjectId == projectId.Value);
            }

            // Filter by date range
            if (fromDate.HasValue)
            {
                query = query.Where(v => v.AppliedDate.Date >= fromDate.Value.Date);
            }

            if (toDate.HasValue)
            {
                query = query.Where(v => v.AppliedDate.Date <= toDate.Value.Date);
            }

            // Filter by status
            if (!string.IsNullOrEmpty(status) && status != "All")
            {
                query = query.Where(v => v.Status == status);
            }

            var volunteers = await query
                .OrderByDescending(v => v.AppliedDate)
                .ToListAsync();

            // Get all projects for dropdown
            ViewBag.Projects = await _context.SDGProjects
                .Where(p => p.IsActive)
                .OrderBy(p => p.Title)
                .Select(p => new { p.Id, p.Title })
                .ToListAsync();

            // Pass filter values back to view
            ViewBag.SelectedProjectId = projectId;
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");
            ViewBag.SelectedStatus = status;

            return View(volunteers);
        }

        [Permission("Volunteers", "View")]
        public async Task<IActionResult> Details(int id)
        {
            var volunteer = await _context.Volunteers
                .Include(v => v.SDGProject)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (volunteer == null)
                return NotFound();

            return View(volunteer);
        }

        [HttpPost]
        [Permission("Volunteers", "Edit")]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer == null)
                return NotFound();

            // Validate status
            if (status == "Pending" || status == "Approved" || status == "Rejected")
            {
                volunteer.Status = status;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Volunteer status updated successfully!";
            }
            else
            {
                TempData["Error"] = "Invalid status value!";
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [Permission("Volunteers", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var volunteer = await _context.Volunteers.FindAsync(id);
            if (volunteer == null)
                return NotFound();

            _context.Volunteers.Remove(volunteer);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Volunteer application deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
