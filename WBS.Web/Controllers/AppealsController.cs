using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;

namespace WBS.Web.Controllers
{
    public class AppealsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AppealsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string? type = null)
        {
            var query = _context.Appeals.Where(a => a.IsActive);

            if (!string.IsNullOrEmpty(type))
            {
                query = query.Where(a => a.AppealType == type);
            }

            var appeals = await query
                .OrderByDescending(a => a.IsUrgent)
                .ThenByDescending(a => a.CreatedAt)
                .ToListAsync();

            ViewBag.Type = type;
            return View(appeals);
        }

        [Route("appeals/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            var appeal = await _context.Appeals
                .FirstOrDefaultAsync(a => a.Slug == slug && a.IsActive);

            if (appeal == null)
                return NotFound();

            return View(appeal);
        }
    }
}
