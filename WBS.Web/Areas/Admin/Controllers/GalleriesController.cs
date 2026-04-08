using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.Services;
using WBS.Web.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Hosting;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class GalleriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;
        private readonly ICloudinaryService _cloudinaryService;
        private readonly ILogger<GalleriesController> _logger;

        public GalleriesController(
            ApplicationDbContext context,
            IWebHostEnvironment environment,
            ICloudinaryService cloudinaryService,
            ILogger<GalleriesController> logger)
        {
            _context = context;
            _environment = environment;
            _cloudinaryService = cloudinaryService;
            _logger = logger;
        }

        // GET: Admin/Galleries
        [Permission("Galleries", "View")]
        public async Task<IActionResult> Index()
        {
            var galleries = await _context.Galleries
                .Include(g => g.Images)
                .OrderByDescending(g => g.CreatedAt)
                .ToListAsync();
            return View(galleries);
        }

        // GET: Admin/Galleries/Details/5
        [Permission("Galleries", "View")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var gallery = await _context.Galleries
                .Include(g => g.Images.OrderBy(i => i.DisplayOrder))
                .FirstOrDefaultAsync(m => m.Id == id);

            if (gallery == null)
                return NotFound();

            return View(gallery);
        }

        // GET: Admin/Galleries/Create
        [Permission("Galleries", "Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Galleries/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Galleries", "Create")]
        public async Task<IActionResult> Create(Gallery gallery, IFormFile? coverImageFile)
        {
            if (ModelState.IsValid)
            {
                // Handle cover image upload to Cloudinary
                if (coverImageFile != null && coverImageFile.Length > 0)
                {
                    _logger.LogInformation("Uploading gallery cover image to Cloudinary");
                    var imageUrl = await _cloudinaryService.UploadImageAsync(coverImageFile, "galleries");
                    
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        gallery.CoverImage = imageUrl;
                        _logger.LogInformation("Gallery cover image uploaded successfully: {ImageUrl}", imageUrl);
                    }
                    else
                    {
                        _logger.LogWarning("Failed to upload gallery cover image to Cloudinary");
                        TempData["Warning"] = "Gallery created but cover image upload failed.";
                    }
                }

                gallery.CreatedAt = DateTime.UtcNow;
                _context.Add(gallery);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Gallery created successfully!";
                return RedirectToAction(nameof(Details), new { id = gallery.Id });
            }
            return View(gallery);
        }

        // GET: Admin/Galleries/Edit/5
        [Permission("Galleries", "Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var gallery = await _context.Galleries
                .Include(g => g.Images.OrderBy(i => i.DisplayOrder))
                .FirstOrDefaultAsync(g => g.Id == id);

            if (gallery == null)
                return NotFound();

            return View(gallery);
        }

        // POST: Admin/Galleries/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Galleries", "Edit")]
        public async Task<IActionResult> Edit(int id, Gallery gallery, IFormFile? coverImageFile)
        {
            if (id != gallery.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.Galleries.FindAsync(id);
                    if (existing == null)
                        return NotFound();

                    // Handle cover image upload to Cloudinary
                    if (coverImageFile != null && coverImageFile.Length > 0)
                    {
                        _logger.LogInformation("Uploading new gallery cover image for Gallery ID: {GalleryId}", id);
                        
                        var imageUrl = await _cloudinaryService.UploadImageAsync(coverImageFile, "galleries");
                        
                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            // Delete old cover image from Cloudinary if exists
                            if (!string.IsNullOrEmpty(existing.CoverImage) && existing.CoverImage.Contains("cloudinary.com"))
                            {
                                try
                                {
                                    var uri = new Uri(existing.CoverImage);
                                    var pathParts = uri.AbsolutePath.Split('/');
                                    var publicIdWithExtension = string.Join("/", pathParts.Skip(pathParts.Length - 2));
                                    var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);
                                    var folder = pathParts[pathParts.Length - 2];
                                    var fullPublicId = $"{folder}/{publicId}";
                                    
                                    await _cloudinaryService.DeleteImageAsync(fullPublicId);
                                }
                                catch (Exception ex)
                                {
                                    _logger.LogWarning(ex, "Failed to delete old cover image");
                                }
                            }
                            
                            gallery.CoverImage = imageUrl;
                            _logger.LogInformation("Gallery cover image updated successfully");
                        }
                        else
                        {
                            TempData["Warning"] = "Gallery updated but cover image upload failed.";
                            gallery.CoverImage = existing.CoverImage;
                        }
                    }
                    else
                    {
                        gallery.CoverImage = existing.CoverImage;
                    }

                    gallery.CreatedAt = existing.CreatedAt;
                    _context.Entry(existing).CurrentValues.SetValues(gallery);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "Gallery updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GalleryExists(gallery.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Details), new { id = gallery.Id });
            }
            return View(gallery);
        }

        // POST: Admin/Galleries/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Galleries", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var gallery = await _context.Galleries
                .Include(g => g.Images)
                .FirstOrDefaultAsync(g => g.Id == id);
                
            if (gallery == null)
                return NotFound();

            // Delete cover image from Cloudinary if exists
            if (!string.IsNullOrEmpty(gallery.CoverImage) && gallery.CoverImage.Contains("cloudinary.com"))
            {
                try
                {
                    var uri = new Uri(gallery.CoverImage);
                    var pathParts = uri.AbsolutePath.Split('/');
                    var publicIdWithExtension = string.Join("/", pathParts.Skip(pathParts.Length - 2));
                    var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);
                    var folder = pathParts[pathParts.Length - 2];
                    var fullPublicId = $"{folder}/{publicId}";
                    
                    await _cloudinaryService.DeleteImageAsync(fullPublicId);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete cover image from Cloudinary");
                }
            }

            // Delete all gallery images from Cloudinary
            foreach (var image in gallery.Images)
            {
                if (!string.IsNullOrEmpty(image.ImageUrl) && image.ImageUrl.Contains("cloudinary.com"))
                {
                    try
                    {
                        var uri = new Uri(image.ImageUrl);
                        var pathParts = uri.AbsolutePath.Split('/');
                        var publicIdWithExtension = string.Join("/", pathParts.Skip(pathParts.Length - 2));
                        var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);
                        var folder = pathParts[pathParts.Length - 2];
                        var fullPublicId = $"{folder}/{publicId}";
                        
                        await _cloudinaryService.DeleteImageAsync(fullPublicId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to delete gallery image from Cloudinary");
                    }
                }
            }

            _context.Galleries.Remove(gallery);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Gallery deleted successfully!";
            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/Galleries/UploadImages/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Galleries", "Edit")]
        public async Task<IActionResult> UploadImages(int id, List<IFormFile> imageFiles, string? caption, string? captionBn)
        {
            var gallery = await _context.Galleries.FindAsync(id);
            if (gallery == null)
                return NotFound();

            if (imageFiles != null && imageFiles.Count > 0)
            {
                // Get current max display order
                var maxOrder = await _context.GalleryImages
                    .Where(g => g.GalleryId == id)
                    .MaxAsync(g => (int?)g.DisplayOrder) ?? 0;

                int uploadedCount = 0;
                foreach (var file in imageFiles)
                {
                    if (file.Length > 0)
                    {
                        _logger.LogInformation("Uploading gallery image for Gallery ID: {GalleryId}", id);
                        var imageUrl = await _cloudinaryService.UploadImageAsync(file, "galleries");
                        
                        if (!string.IsNullOrEmpty(imageUrl))
                        {
                            var galleryImage = new GalleryImage
                            {
                                GalleryId = id,
                                ImageUrl = imageUrl,
                                Caption = caption,
                                CaptionBn = captionBn,
                                DisplayOrder = ++maxOrder,
                                IsActive = true,
                                CreatedAt = DateTime.UtcNow
                            };

                            _context.GalleryImages.Add(galleryImage);
                            uploadedCount++;
                        }
                        else
                        {
                            _logger.LogWarning("Failed to upload gallery image");
                        }
                    }
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = $"{uploadedCount} image(s) uploaded successfully!";
                
                if (uploadedCount < imageFiles.Count)
                {
                    TempData["Warning"] = $"{imageFiles.Count - uploadedCount} image(s) failed to upload.";
                }
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: Admin/Galleries/DeleteImage/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Galleries", "Delete")]
        public async Task<IActionResult> DeleteImage(int id, int galleryId)
        {
            var image = await _context.GalleryImages.FindAsync(id);
            if (image == null)
                return NotFound();

            // Delete image from Cloudinary if exists
            if (!string.IsNullOrEmpty(image.ImageUrl) && image.ImageUrl.Contains("cloudinary.com"))
            {
                try
                {
                    var uri = new Uri(image.ImageUrl);
                    var pathParts = uri.AbsolutePath.Split('/');
                    var publicIdWithExtension = string.Join("/", pathParts.Skip(pathParts.Length - 2));
                    var publicId = Path.GetFileNameWithoutExtension(publicIdWithExtension);
                    var folder = pathParts[pathParts.Length - 2];
                    var fullPublicId = $"{folder}/{publicId}";
                    
                    await _cloudinaryService.DeleteImageAsync(fullPublicId);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Failed to delete gallery image from Cloudinary");
                }
            }

            _context.GalleryImages.Remove(image);
            await _context.SaveChangesAsync();

            TempData["Success"] = "Image deleted successfully!";
            return RedirectToAction(nameof(Details), new { id = galleryId });
        }

        // POST: Admin/Galleries/UpdateImageOrder
        [HttpPost]
        [Permission("Galleries", "Edit")]
        public async Task<IActionResult> UpdateImageOrder(int imageId, int newOrder)
        {
            var image = await _context.GalleryImages.FindAsync(imageId);
            if (image == null)
                return Json(new { success = false, message = "Image not found" });

            image.DisplayOrder = newOrder;
            await _context.SaveChangesAsync();

            return Json(new { success = true });
        }

        private bool GalleryExists(int id)
        {
            return _context.Galleries.Any(e => e.Id == id);
        }
    }
}
