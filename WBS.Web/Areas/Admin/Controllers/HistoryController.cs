using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.Attributes;
using WBS.Web.Services;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HistoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ILogger<HistoryController> _logger;

        public HistoryController(
            ApplicationDbContext context, 
            IWebHostEnvironment environment,
            ICloudinaryService cloudinaryService,
            ILogger<HistoryController> logger)
        {
            _context = context;
            _environment = environment;
            _cloudinaryService = cloudinaryService;
            _logger = logger;
        }

        // GET: Admin/History
        [Permission("History", "View")]
        public async Task<IActionResult> Index()
        {
            var histories = await _context.Histories
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync();

            return View(histories);
        }

        // GET: Admin/History/Create
        [Permission("History", "Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/History/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("History", "Create")]
        public async Task<IActionResult> Create(History history, IFormFile? featuredImage)
        {
            // Remove FeaturedImage from ModelState validation since it's handled separately
            ModelState.Remove("FeaturedImage");

            if (featuredImage == null)
            {
                ModelState.AddModelError("featuredImage", "Featured image is required.");
            }

            if (ModelState.IsValid && featuredImage != null)
            {
                // Upload featured image to Cloudinary
                _logger.LogInformation("Uploading history featured image to Cloudinary");
                var imageUrl = await _cloudinaryService.UploadImageAsync(featuredImage, "history");
                
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    history.FeaturedImage = imageUrl;
                    _logger.LogInformation("History featured image uploaded successfully to Cloudinary: {ImageUrl}", imageUrl);
                }
                else
                {
                    _logger.LogWarning("Failed to upload history featured image to Cloudinary");
                    ModelState.AddModelError("featuredImage", "Failed to upload image. Please try again.");
                    return View(history);
                }

                history.CreatedAt = DateTime.UtcNow;
                _context.Histories.Add(history);
                await _context.SaveChangesAsync();

                TempData["Success"] = "History created successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(history);
        }

        // GET: Admin/History/Edit/5
        [Permission("History", "Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var history = await _context.Histories.FindAsync(id);
            if (history == null)
                return NotFound();

            return View(history);
        }

        // POST: Admin/History/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("History", "Edit")]
        public async Task<IActionResult> Edit(int id, History history, IFormFile? featuredImage)
        {
            if (id != history.Id)
                return NotFound();

            // Remove FeaturedImage from ModelState validation since it's handled separately
            ModelState.Remove("FeaturedImage");

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.Histories.FindAsync(id);
                    if (existing == null)
                        return NotFound();

                    // Upload new featured image to Cloudinary if provided
                    if (featuredImage != null && featuredImage.Length > 0)
                    {
                        _logger.LogInformation("Uploading new history featured image to Cloudinary for History ID: {HistoryId}", id);
                        
                        var imageUrl = await _cloudinaryService.UploadImageAsync(featuredImage, "history");
                        
                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            // Delete old image from Cloudinary if it exists
                            if (!string.IsNullOrEmpty(existing.FeaturedImage) && existing.FeaturedImage.Contains("cloudinary.com"))
                            {
                                try
                                {
                                    var uri = new Uri(existing.FeaturedImage);
                                    var pathParts = uri.AbsolutePath.Split('/');
                                    var publicIdWithExtension = string.Join("/", pathParts.Skip(pathParts.Length - 2));
                                    var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);
                                    var folder = pathParts[pathParts.Length - 2];
                                    var fullPublicId = $"{folder}/{publicId}";
                                    
                                    _logger.LogInformation("Attempting to delete old history image from Cloudinary: {PublicId}", fullPublicId);
                                    await _cloudinaryService.DeleteImageAsync(fullPublicId);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogWarning(ex, "Failed to delete old history image from Cloudinary");
                                }
                            }
                            // Delete old local image if it exists
                            else if (!string.IsNullOrEmpty(existing.FeaturedImage) && !existing.FeaturedImage.Contains("cloudinary.com"))
                            {
                                var oldPath = Path.Combine(_environment.WebRootPath, existing.FeaturedImage.TrimStart('/'));
                                if (System.IO.File.Exists(oldPath))
                                {
                                    try
                                    {
                                        System.IO.File.Delete(oldPath);
                                        _logger.LogInformation("Deleted old local history image: {Path}", oldPath);
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogWarning(ex, "Failed to delete old local history image");
                                    }
                                }
                            }
                            
                            history.FeaturedImage = imageUrl;
                            _logger.LogInformation("History featured image updated successfully in Cloudinary: {ImageUrl}", imageUrl);
                        }
                        else
                        {
                            _logger.LogWarning("Failed to upload new history featured image to Cloudinary, keeping old image");
                            TempData["Warning"] = "History updated but image upload failed. Please try uploading the image again.";
                            history.FeaturedImage = existing.FeaturedImage;
                        }
                    }
                    else
                    {
                        history.FeaturedImage = existing.FeaturedImage;
                    }

                    history.CreatedAt = existing.CreatedAt;
                    history.UpdatedAt = DateTime.UtcNow;
                    _context.Entry(existing).CurrentValues.SetValues(history);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "History updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HistoryExists(history.Id))
                        return NotFound();
                    throw;
                }
            }

            return View(history);
        }

        // POST: Admin/History/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var history = await _context.Histories.FindAsync(id);
            if (history != null)
            {
                // Delete image from Cloudinary if exists
                if (!string.IsNullOrEmpty(history.FeaturedImage))
                {
                    if (history.FeaturedImage.Contains("cloudinary.com"))
                    {
                        try
                        {
                            var uri = new Uri(history.FeaturedImage);
                            var pathParts = uri.AbsolutePath.Split('/');
                            var publicIdWithExtension = string.Join("/", pathParts.Skip(pathParts.Length - 2));
                            var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);
                            var folder = pathParts[pathParts.Length - 2];
                            var fullPublicId = $"{folder}/{publicId}";
                            
                            _logger.LogInformation("Deleting history image from Cloudinary: {PublicId}", fullPublicId);
                            await _cloudinaryService.DeleteImageAsync(fullPublicId);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogWarning(ex, "Failed to delete history image from Cloudinary");
                        }
                    }
                    // Delete local image if exists
                    else
                    {
                        var filePath = Path.Combine(_environment.WebRootPath, history.FeaturedImage.TrimStart('/'));
                        if (System.IO.File.Exists(filePath))
                        {
                            try
                            {
                                System.IO.File.Delete(filePath);
                                _logger.LogInformation("Deleted local history image: {Path}", filePath);
                            }
                            catch (Exception ex)
                            {
                                _logger.LogWarning(ex, "Failed to delete local history image");
                            }
                        }
                    }
                }

                _context.Histories.Remove(history);
                await _context.SaveChangesAsync();
                TempData["Success"] = "History deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/History/ToggleActive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var history = await _context.Histories.FindAsync(id);
            if (history != null)
            {
                history.IsActive = !history.IsActive;
                history.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                TempData["Success"] = $"History {(history.IsActive ? "activated" : "deactivated")} successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool HistoryExists(int id)
        {
            return _context.Histories.Any(e => e.Id == id);
        }
    }
}

