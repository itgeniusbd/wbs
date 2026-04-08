using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using WBS.Web.Attributes;
using WBS.Web.Data;
using WBS.Web.Models;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class VideoGalleriesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public VideoGalleriesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Permission("Galleries", "View")]
        public async Task<IActionResult> Index()
        {
            var videos = await _context.VideoGalleries
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync();
            return View(videos);
        }

        [Permission("Galleries", "Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Galleries", "Create")]
        public async Task<IActionResult> Create(VideoGallery video)
        {
            if (ModelState.IsValid)
            {
                // Extract YouTube video ID from URL
                video.YouTubeVideoId = ExtractYouTubeVideoId(video.YouTubeUrl);
                
                // Set thumbnail URL from YouTube
                if (!string.IsNullOrEmpty(video.YouTubeVideoId))
                {
                    video.ThumbnailUrl = $"https://img.youtube.com/vi/{video.YouTubeVideoId}/maxresdefault.jpg";
                }

                video.CreatedAt = DateTime.UtcNow;
                video.CreatedBy = User.Identity?.Name;
                
                _context.VideoGalleries.Add(video);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Video added successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(video);
        }

        [Permission("Galleries", "Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var video = await _context.VideoGalleries.FindAsync(id);
            if (video == null)
                return NotFound();

            return View(video);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Galleries", "Edit")]
        public async Task<IActionResult> Edit(int id, VideoGallery video)
        {
            if (id != video.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.VideoGalleries.FindAsync(id);
                    if (existing == null)
                        return NotFound();

                    // Extract YouTube video ID from URL
                    video.YouTubeVideoId = ExtractYouTubeVideoId(video.YouTubeUrl);
                    
                    // Set thumbnail URL from YouTube
                    if (!string.IsNullOrEmpty(video.YouTubeVideoId))
                    {
                        video.ThumbnailUrl = $"https://img.youtube.com/vi/{video.YouTubeVideoId}/maxresdefault.jpg";
                    }

                    video.CreatedAt = existing.CreatedAt;
                    video.CreatedBy = existing.CreatedBy;
                    video.UpdatedAt = DateTime.UtcNow;

                    _context.Entry(existing).CurrentValues.SetValues(video);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Video updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VideoGalleryExists(video.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(video);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Galleries", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var video = await _context.VideoGalleries.FindAsync(id);
            if (video == null)
                return NotFound();

            _context.VideoGalleries.Remove(video);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Video deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [Permission("Galleries", "Edit")]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var video = await _context.VideoGalleries.FindAsync(id);
            if (video == null)
                return NotFound();

            video.IsActive = !video.IsActive;
            await _context.SaveChangesAsync();

            return Json(new { success = true, isActive = video.IsActive });
        }

        [HttpPost]
        [Permission("Galleries", "Edit")]
        public async Task<IActionResult> ToggleFeatured(int id)
        {
            var video = await _context.VideoGalleries.FindAsync(id);
            if (video == null)
                return NotFound();

            video.IsFeatured = !video.IsFeatured;
            await _context.SaveChangesAsync();

            return Json(new { success = true, isFeatured = video.IsFeatured });
        }

        private bool VideoGalleryExists(int id)
        {
            return _context.VideoGalleries.Any(e => e.Id == id);
        }

        private string? ExtractYouTubeVideoId(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            // Match different YouTube URL formats
            var patterns = new[]
            {
                @"(?:https?:\/\/)?(?:www\.)?youtube\.com\/watch\?v=([a-zA-Z0-9_-]{11})",
                @"(?:https?:\/\/)?(?:www\.)?youtu\.be\/([a-zA-Z0-9_-]{11})",
                @"(?:https?:\/\/)?(?:www\.)?youtube\.com\/embed\/([a-zA-Z0-9_-]{11})",
                @"(?:https?:\/\/)?(?:www\.)?youtube\.com\/v\/([a-zA-Z0-9_-]{11})"
            };

            foreach (var pattern in patterns)
            {
                var match = Regex.Match(url, pattern);
                if (match.Success)
                {
                    return match.Groups[1].Value;
                }
            }

            return null;
        }
    }
}
