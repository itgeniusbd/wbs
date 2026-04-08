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
    public class SlidersController : Controller
    {
        private readonly IContentService _contentService;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SlidersController> _logger;

        public SlidersController(IContentService contentService, ICloudinaryService cloudinaryService, ApplicationDbContext context, ILogger<SlidersController> logger)
        {
            _contentService = contentService;
            _cloudinaryService = cloudinaryService;
            _context = context;
            _logger = logger;
        }

        [Permission("Sliders", "View")]
        public async Task<IActionResult> Index()
        {
            var sliders = await _contentService.GetAllSlidersAsync();
            return View(sliders);
        }

        [Permission("Sliders", "Create")]
        public IActionResult Create()
        {
            return View(new Slider());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Sliders", "Create")]
        public async Task<IActionResult> Create(Slider slider, IFormFile imageFile)
        {
            try
            {
                _logger.LogInformation("=== Starting slider creation ===");
                _logger.LogInformation("Title: {Title}", slider.Title);
                _logger.LogInformation("TitleBn: {TitleBn}", slider.TitleBn);
                _logger.LogInformation("Image file: {FileName}", imageFile?.FileName ?? "NULL");

                if (imageFile == null || imageFile.Length == 0)
                {
                    _logger.LogWarning("No image file provided");
                    ModelState.AddModelError("imageFile", "Slider image is required");
                    ViewBag.ErrorMessage = "Please select an image file";
                    return View(slider);
                }

                if (string.IsNullOrWhiteSpace(slider.Title))
                {
                    _logger.LogWarning("Title is empty");
                    ModelState.AddModelError("Title", "Title is required");
                    ViewBag.ErrorMessage = "Please enter a title";
                    return View(slider);
                }

                // Validate image type
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    _logger.LogWarning("Invalid image type: {Extension}", extension);
                    ModelState.AddModelError("imageFile", "Only JPG, JPEG, PNG, GIF, and WEBP files are allowed");
                    ViewBag.ErrorMessage = "Invalid image file type. Please use JPG, PNG, GIF, or WEBP";
                    return View(slider);
                }

                // Validate content type
                var allowedContentTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
                if (!allowedContentTypes.Contains(imageFile.ContentType.ToLowerInvariant()))
                {
                    _logger.LogWarning("Invalid content type: {ContentType}", imageFile.ContentType);
                    ModelState.AddModelError("imageFile", "Invalid image file format");
                    ViewBag.ErrorMessage = "Invalid image file format. Please upload a valid image file.";
                    return View(slider);
                }

                // Validate image size (max 10MB)
                if (imageFile.Length > 10 * 1024 * 1024)
                {
                    _logger.LogWarning("Image file too large: {Size} bytes", imageFile.Length);
                    ModelState.AddModelError("imageFile", "Image size must be less than 10MB");
                    ViewBag.ErrorMessage = "Image file is too large. Maximum size is 10MB";
                    return View(slider);
                }

                _logger.LogInformation("Uploading image to Cloudinary: {FileName}, Size: {Size} bytes, ContentType: {ContentType}", 
                    imageFile.FileName, imageFile.Length, imageFile.ContentType);
                var uploadedImageUrl = await _cloudinaryService.UploadImageAsync(imageFile, "sliders");
                
                if (string.IsNullOrEmpty(uploadedImageUrl))
                {
                    _logger.LogError("Failed to upload image to Cloudinary");
                    ModelState.AddModelError("imageFile", "Failed to upload image. Please try again.");
                    ViewBag.ErrorMessage = @"Failed to upload image to Cloudinary. This could be due to:
                        <ul class='mb-0'>
                            <li>Internet connection issues - Please check your network connection</li>
                            <li>Cloudinary API credentials - Verify your API key and secret in appsettings.json</li>
                            <li>Firewall or proxy blocking Cloudinary API</li>
                            <li>Cloudinary service temporarily unavailable</li>
                        </ul>
                        <strong>Check the application logs for detailed error information.</strong>";
                    return View(slider);
                }
                
                slider.ImageUrl = uploadedImageUrl;
                _logger.LogInformation("Image uploaded successfully to: {ImageUrl}", slider.ImageUrl);

                _logger.LogInformation("Creating slider in database...");
                await _contentService.CreateSliderAsync(slider);
                _logger.LogInformation("Slider created successfully with ID: {Id}", slider.Id);

                TempData["Success"] = $"Slider '{slider.Title}' has been created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, "Database error while creating slider");
                var innerMessage = dbEx.InnerException?.Message ?? dbEx.Message;
                ViewBag.ErrorMessage = $"Database error: {innerMessage}";
                ModelState.AddModelError("", "An error occurred while saving the entity changes. See the inner exception for details.");
                return View(slider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating slider");
                ViewBag.ErrorMessage = $"An error occurred while creating the slider: {ex.Message}. Please check the logs for more details.";
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                return View(slider);
            }
        }

        [Permission("Sliders", "Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
                return NotFound();

            return View(slider);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Sliders", "Edit")]
        public async Task<IActionResult> Edit(int id, Slider slider, IFormFile? imageFile)
        {
            try
            {
                _logger.LogInformation("=== Starting slider edit for ID: {Id} ===", id);
                
                if (id != slider.Id)
                {
                    _logger.LogWarning("ID mismatch: {UrlId} vs {ModelId}", id, slider.Id);
                    return NotFound();
                }

                var existing = await _context.Sliders.FindAsync(id);
                if (existing == null)
                {
                    _logger.LogWarning("Slider not found: {Id}", id);
                    return NotFound();
                }

                // Remove ImageUrl from ModelState validation since it's handled separately
                ModelState.Remove("ImageUrl");

                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Model validation failed");
                    foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                    {
                        _logger.LogWarning("Validation error: {ErrorMessage}", error.ErrorMessage);
                    }
                    ViewBag.ErrorMessage = "Please correct the validation errors.";
                    return View(slider);
                }

                // Handle image upload
                if (imageFile != null && imageFile.Length > 0)
                {
                    _logger.LogInformation("New image uploaded: {FileName}, Size: {Size}, ContentType: {ContentType}", 
                        imageFile.FileName, imageFile.Length, imageFile.ContentType);
                    
                    // Validate image type
                    var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                    var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
                    if (!allowedExtensions.Contains(extension))
                    {
                        _logger.LogWarning("Invalid image type: {Extension}", extension);
                        ModelState.AddModelError("imageFile", "Only JPG, JPEG, PNG, GIF, and WEBP files are allowed");
                        ViewBag.ErrorMessage = "Invalid image file type. Only JPG, JPEG, PNG, GIF, and WEBP files are allowed";
                        slider.ImageUrl = existing.ImageUrl;
                        return View(slider);
                    }

                    // Validate content type
                    var allowedContentTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/webp" };
                    if (!allowedContentTypes.Contains(imageFile.ContentType.ToLowerInvariant()))
                    {
                        _logger.LogWarning("Invalid content type: {ContentType}", imageFile.ContentType);
                        ModelState.AddModelError("imageFile", "Invalid image file format");
                        ViewBag.ErrorMessage = "Invalid image file format. Please upload a valid image file.";
                        slider.ImageUrl = existing.ImageUrl;
                        return View(slider);
                    }

                    // Validate image size (max 10MB)
                    if (imageFile.Length > 10 * 1024 * 1024)
                    {
                        _logger.LogWarning("Image file too large: {Size} bytes", imageFile.Length);
                        ModelState.AddModelError("imageFile", "Image size must be less than 10MB");
                        ViewBag.ErrorMessage = "Image file is too large. Maximum size is 10MB";
                        slider.ImageUrl = existing.ImageUrl;
                        return View(slider);
                    }

                    _logger.LogInformation("Starting Cloudinary upload for slider {Id}...", id);
                    var uploadedImageUrl = await _cloudinaryService.UploadImageAsync(imageFile, "sliders");
                    
                    if (string.IsNullOrEmpty(uploadedImageUrl))
                    {
                        _logger.LogError("Failed to upload image to Cloudinary for slider {Id}", id);
                        ModelState.AddModelError("imageFile", "Failed to upload image to Cloudinary. Please check the logs for details.");
                        ViewBag.ErrorMessage = @"Failed to upload image to Cloudinary. This could be due to:
                            <ul class='mb-0'>
                                <li>Internet connection issues - Please check your network connection</li>
                                <li>Cloudinary API credentials - Verify your API key and secret in appsettings.json</li>
                                <li>Firewall or proxy blocking Cloudinary API</li>
                                <li>Cloudinary service temporarily unavailable</li>
                            </ul>
                            <strong>Check the application logs for detailed error information.</strong>";
                        slider.ImageUrl = existing.ImageUrl;
                        return View(slider);
                    }
                    
                    slider.ImageUrl = uploadedImageUrl;
                    _logger.LogInformation("New image uploaded to Cloudinary: {ImageUrl}", slider.ImageUrl);
                }
                else
                {
                    // Keep existing image
                    slider.ImageUrl = existing.ImageUrl;
                    _logger.LogInformation("Keeping existing image: {ImageUrl}", existing.ImageUrl);
                }

                // Update properties
                existing.Title = slider.Title;
                existing.TitleBn = slider.TitleBn;
                existing.Subtitle = slider.Subtitle;
                existing.SubtitleBn = slider.SubtitleBn;
                existing.ImageUrl = slider.ImageUrl;
                existing.ButtonText = slider.ButtonText;
                existing.ButtonTextBn = slider.ButtonTextBn;
                existing.ButtonUrl = slider.ButtonUrl;
                existing.SecondButtonText = slider.SecondButtonText;
                existing.SecondButtonUrl = slider.SecondButtonUrl;
                existing.DisplayOrder = slider.DisplayOrder;
                existing.IsActive = slider.IsActive;

                await _context.SaveChangesAsync();
                _logger.LogInformation("Slider updated successfully: {Id}", id);

                TempData["Success"] = $"Slider '{slider.Title}' has been updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating slider {Id}", id);
                ViewBag.ErrorMessage = $"An unexpected error occurred: {ex.Message}. Please check the logs for more details.";
                ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
                
                // Load existing data to show in form
                var existing = await _context.Sliders.FindAsync(id);
                if (existing != null)
                {
                    slider.ImageUrl = existing.ImageUrl;
                }
                
                return View(slider);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Sliders", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _contentService.DeleteSliderAsync(id);
            TempData["Success"] = "Slider deleted successfully!";
            return RedirectToAction(nameof(Index));
        }
    }
}
