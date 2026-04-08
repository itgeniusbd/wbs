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
    public class NewsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;

        public NewsController(ApplicationDbContext context, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        [Permission("News", "View")]
        public async Task<IActionResult> Index()
        {
            var news = await _context.News
                .OrderByDescending(n => n.PublishedDate)
                .ToListAsync();
            return View(news);
        }

        [Permission("News", "Create")]
        public IActionResult Create()
        {
            return View(new News());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("News", "Create")]
        public async Task<IActionResult> Create(News news, IFormFile? featuredImage)
        {
            // Generate slug before validation if Title is provided
            if (!string.IsNullOrEmpty(news.Title) && string.IsNullOrEmpty(news.Slug))
            {
                news.Slug = GenerateSlug(news.Title);
            }

            if (!ModelState.IsValid)
                return View(news);

            if (featuredImage != null)
            {
                news.FeaturedImage = await _cloudinaryService.UploadImageAsync(featuredImage, "news");
            }

            news.CreatedAt = DateTime.UtcNow;
            news.PublishedDate = DateTime.UtcNow;

            _context.News.Add(news);
            await _context.SaveChangesAsync();

            TempData["Success"] = "News created successfully!";
            return RedirectToAction(nameof(Index));
        }

        [Permission("News", "Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
                return NotFound();

            return View(news);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("News", "Edit")]
        public async Task<IActionResult> Edit(int id, News news, IFormFile? featuredImage)
        {
            if (id != news.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(news);

            var existing = await _context.News.FindAsync(id);
            if (existing == null)
                return NotFound();

            if (featuredImage != null)
            {
                news.FeaturedImage = await _cloudinaryService.UploadImageAsync(featuredImage, "news");
            }
            else
            {
                news.FeaturedImage = existing.FeaturedImage;
            }

            news.Slug = existing.Slug;
            news.CreatedAt = existing.CreatedAt;
            news.UpdatedAt = DateTime.UtcNow;

            _context.Entry(existing).CurrentValues.SetValues(news);
            await _context.SaveChangesAsync();

            TempData["Success"] = "News updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("News", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news != null)
            {
                _context.News.Remove(news);
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "News deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        private static string GenerateSlug(string title)
        {
            var slug = title.ToLower().Replace(" ", "-").Replace("&", "and");
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\-]", "");
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"-+", "-");
            return slug.Trim('-');
        }
    }
}
