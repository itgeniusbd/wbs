using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;

namespace WBS.Web.Controllers
{
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var pageSize = 12;
            var news = await _context.News
                .Where(n => n.IsActive)
                .OrderByDescending(n => n.PublishedDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling(await _context.News.CountAsync(n => n.IsActive) / (double)pageSize);

            return View(news);
        }

        [Route("news/{slug}")]
        public async Task<IActionResult> Details(string slug)
        {
            var news = await _context.News
                .FirstOrDefaultAsync(n => n.Slug == slug && n.IsActive);

            if (news == null)
                return NotFound();

            // Increment view count
            news.ViewCount++;
            _context.Update(news);
            await _context.SaveChangesAsync();

            return View(news);
        }
    }
}
