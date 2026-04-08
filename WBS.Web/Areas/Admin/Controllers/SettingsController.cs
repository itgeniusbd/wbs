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
    public class SettingsController : Controller
    {
        private readonly IContentService _contentService;
        private readonly IWebHostEnvironment _environment;
        private readonly ApplicationDbContext _context;

        public SettingsController(IContentService contentService, IWebHostEnvironment environment, ApplicationDbContext context)
        {
            _contentService = contentService;
            _environment = environment;
            _context = context;
        }

        [Permission("Settings", "View")]
        public async Task<IActionResult> Index()
        {
            var settings = await _contentService.GetSiteSettingsAsync();
            if (settings == null)
            {
                settings = new SiteSettings();
            }
            return View(settings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Settings", "Edit")]
        public async Task<IActionResult> Index(SiteSettings settings, IFormFile? logoFile, IFormFile? logoWhiteFile, IFormFile? faviconFile, IFormFile? aboutUsImageFile, IFormFile? paymentBannerFile)
        {
            if (!ModelState.IsValid)
                return View(settings);

            var existing = await _contentService.GetSiteSettingsAsync();

            if (logoFile != null)
            {
                settings.Logo = await SaveImage(logoFile, "settings");
            }
            else if (existing != null)
            {
                settings.Logo = existing.Logo;
            }

            if (logoWhiteFile != null)
            {
                settings.LogoWhite = await SaveImage(logoWhiteFile, "settings");
            }
            else if (existing != null)
            {
                settings.LogoWhite = existing.LogoWhite;
            }

            if (faviconFile != null)
            {
                settings.Favicon = await SaveImage(faviconFile, "settings");
            }
            else if (existing != null)
            {
                settings.Favicon = existing.Favicon;
            }

            if (aboutUsImageFile != null)
            {
                settings.AboutUsImage = await SaveImage(aboutUsImageFile, "settings");
            }
            else if (existing != null)
            {
                settings.AboutUsImage = existing.AboutUsImage;
            }

            if (paymentBannerFile != null)
            {
                settings.PaymentGatewayBanner = await SaveImage(paymentBannerFile, "settings");
            }
            else if (existing != null)
            {
                settings.PaymentGatewayBanner = existing.PaymentGatewayBanner;
            }

            if (existing != null)
            {
                settings.Id = existing.Id;
            }

            await _contentService.UpdateSiteSettingsAsync(settings);

            TempData["Success"] = "Settings updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> SaveImage(IFormFile file, string folder)
        {
            var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", folder);
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/uploads/{folder}/{fileName}";
        }
    }
}
