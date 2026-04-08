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
    public class AppealsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ICloudinaryService _cloudinaryService;

        public AppealsController(ApplicationDbContext context, ICloudinaryService cloudinaryService)
        {
            _context = context;
            _cloudinaryService = cloudinaryService;
        }

        [Permission("Appeals", "View")]
        public async Task<IActionResult> Index()
        {
            var appeals = await _context.Appeals
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
            return View(appeals);
        }

        [Permission("Appeals", "Create")]
        public IActionResult Create()
        {
            return View(new Appeal());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Appeals", "Create")]
        public async Task<IActionResult> Create(Appeal appeal, IFormFile? featuredImage, IFormFile? bannerImage)
        {
            try
            {
                // Remove model state errors for properties we don't need validation on
                ModelState.Remove("Slug");
                ModelState.Remove("CreatedAt");
                ModelState.Remove("UpdatedAt");
                ModelState.Remove("Donations");
                
                // Validate featured image
                if (featuredImage == null)
                {
                    ModelState.AddModelError("featuredImage", "Featured image is required.");
                }

                if (!ModelState.IsValid)
                {
                    // Log errors for debugging
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    TempData["Error"] = "Please fix validation errors: " + string.Join(", ", errors);
                    return View(appeal);
                }

                // Save featured image
                if (featuredImage != null)
                {
                    appeal.FeaturedImage = await _cloudinaryService.UploadImageAsync(featuredImage, "appeals");
                }

                // Save banner image if provided
                if (bannerImage != null)
                {
                    appeal.BannerImage = await _cloudinaryService.UploadImageAsync(bannerImage, "appeals/banners");
                }

                // Generate slug and set timestamps
                appeal.Slug = GenerateSlug(appeal.Title);
                appeal.CreatedAt = DateTime.UtcNow;
                appeal.RaisedAmount = 0;

                // Save to database
                _context.Appeals.Add(appeal);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Appeal created successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log the error
                TempData["Error"] = $"Error creating appeal: {ex.Message}";
                return View(appeal);
            }
        }

        [Permission("Appeals", "Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var appeal = await _context.Appeals.FindAsync(id);
            if (appeal == null)
                return NotFound();

            return View(appeal);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Appeals", "Edit")]
        public async Task<IActionResult> Edit(int id, Appeal appeal, IFormFile? featuredImage, IFormFile? bannerImage)
        {
            if (id != appeal.Id)
                return NotFound();

            try
            {
                // Remove validation errors for properties that are auto-set or not editable
                ModelState.Remove("Slug");
                ModelState.Remove("CreatedAt");
                ModelState.Remove("UpdatedAt");
                ModelState.Remove("Donations");
                ModelState.Remove("RaisedAmount");
                ModelState.Remove("FeaturedImage");
                ModelState.Remove("BannerImage");

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    TempData["Error"] = "Please fix validation errors: " + string.Join(", ", errors);
                    return View(appeal);
                }

                var existing = await _context.Appeals.FindAsync(id);
                if (existing == null)
                    return NotFound();

                // Handle featured image upload
                if (featuredImage != null)
                {
                    appeal.FeaturedImage = await _cloudinaryService.UploadImageAsync(featuredImage, "appeals");
                }
                else
                {
                    appeal.FeaturedImage = existing.FeaturedImage;
                }

                // Handle banner image upload
                if (bannerImage != null)
                {
                    appeal.BannerImage = await _cloudinaryService.UploadImageAsync(bannerImage, "appeals/banners");
                }
                else
                {
                    appeal.BannerImage = existing.BannerImage;
                }

                // Preserve immutable fields
                appeal.Slug = existing.Slug;
                appeal.CreatedAt = existing.CreatedAt;
                appeal.UpdatedAt = DateTime.UtcNow;
                appeal.RaisedAmount = existing.RaisedAmount;

                // Update entity
                _context.Entry(existing).CurrentValues.SetValues(appeal);
                await _context.SaveChangesAsync();

                TempData["Success"] = "Appeal updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error updating appeal: {ex.Message}";
                return View(appeal);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Appeals", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var appeal = await _context.Appeals.FindAsync(id);
            if (appeal != null)
            {
                _context.Appeals.Remove(appeal);
                await _context.SaveChangesAsync();
            }

            TempData["Success"] = "Appeal deleted successfully!";
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

