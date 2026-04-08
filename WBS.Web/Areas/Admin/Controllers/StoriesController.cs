using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Attributes;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.Services;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class StoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;

        public StoriesController(ApplicationDbContext context, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        // GET: Admin/Stories
        [Permission("Success Stories", "View")]
        public async Task<IActionResult> Index()
        {
            var stories = await _context.Stories
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync();

            return View(stories);
        }

        // GET: Admin/Stories/Create
        [Permission("Success Stories", "Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Stories/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Success Stories", "Create")]
        public async Task<IActionResult> Create(Story story, IFormFile? featuredImage)
        {
            if (ModelState.IsValid)
            {
                // Upload featured image if provided
                if (featuredImage != null)
                {
                    story.FeaturedImage = await _cloudinaryService.UploadImageAsync(featuredImage, "stories");
                }

                // Generate slug if not provided
                if (string.IsNullOrEmpty(story.Slug))
                {
                    story.Slug = GenerateSlug(story.Title);
                }

                story.CreatedAt = DateTime.UtcNow;
                _context.Stories.Add(story);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Success story created successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(story);
        }

        // GET: Admin/Stories/Edit/5
        [Permission("Success Stories", "Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var story = await _context.Stories.FindAsync(id);
            if (story == null)
                return NotFound();

            return View(story);
        }

        // POST: Admin/Stories/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Success Stories", "Edit")]
        public async Task<IActionResult> Edit(int id, Story story, IFormFile? featuredImage)
        {
            if (id != story.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.Stories.FindAsync(id);
                    if (existing == null)
                        return NotFound();

                    // Upload new featured image if provided
                    if (featuredImage != null)
                    {
                        story.FeaturedImage = await _cloudinaryService.UploadImageAsync(featuredImage, "stories");
                    }
                    else
                    {
                        story.FeaturedImage = existing.FeaturedImage;
                    }

                    // Generate slug if changed
                    if (string.IsNullOrEmpty(story.Slug))
                    {
                        story.Slug = GenerateSlug(story.Title);
                    }

                    story.CreatedAt = existing.CreatedAt;
                    _context.Entry(existing).CurrentValues.SetValues(story);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Success story updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StoryExists(story.Id))
                        return NotFound();
                    throw;
                }
            }

            return View(story);
        }

        // POST: Admin/Stories/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Success Stories", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var story = await _context.Stories.FindAsync(id);
            if (story != null)
            {
                _context.Stories.Remove(story);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Success story deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Stories/ToggleActive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var story = await _context.Stories.FindAsync(id);
            if (story != null)
            {
                story.IsActive = !story.IsActive;
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Story {(story.IsActive ? "activated" : "deactivated")} successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Stories/ToggleFeatured/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFeatured(int id)
        {
            var story = await _context.Stories.FindAsync(id);
            if (story != null)
            {
                story.IsFeatured = !story.IsFeatured;
                await _context.SaveChangesAsync();
                TempData["Success"] = $"Story {(story.IsFeatured ? "marked as featured" : "removed from featured")} successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool StoryExists(int id)
        {
            return _context.Stories.Any(e => e.Id == id);
        }

        private string GenerateSlug(string title)
        {
            var slug = title.ToLower()
                .Replace(" ", "-")
                .Replace("'", "")
                .Replace("\"", "");

            // Remove special characters
            slug = System.Text.RegularExpressions.Regex.Replace(slug, @"[^a-z0-9\-]", "");

            // Ensure uniqueness
            var existingSlug = _context.Stories.FirstOrDefault(s => s.Slug == slug);
            if (existingSlug != null)
            {
                slug = $"{slug}-{DateTime.UtcNow.Ticks}";
            }

            return slug;
        }
    }
}
