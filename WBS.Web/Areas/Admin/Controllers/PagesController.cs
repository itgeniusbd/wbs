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
    public class PagesController : Controller
    {
        private readonly IPageService _pageService;
        private readonly IWebHostEnvironment _environment;

        public PagesController(IPageService pageService, IWebHostEnvironment environment)
        {
            _pageService = pageService;
            _environment = environment;
        }

        [Permission("Pages", "View")]
        public async Task<IActionResult> Index()
        {
            var pages = await _pageService.GetAllPagesAsync();
            return View(pages);
        }

        [Permission("Pages", "Create")]
        public IActionResult Create()
        {
            return View(new Page());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Pages", "Create")]
        public async Task<IActionResult> Create(Page page, IFormFile? featuredImage, IFormFile? bannerImage)
        {
            if (!ModelState.IsValid)
                return View(page);

            if (featuredImage != null)
            {
                page.FeaturedImage = await SaveImage(featuredImage, "pages");
            }

            if (bannerImage != null)
            {
                page.BannerImage = await SaveImage(bannerImage, "pages/banners");
            }

            page.CreatedBy = User.Identity?.Name;
            await _pageService.CreatePageAsync(page);

            TempData["Success"] = "Page created successfully!";
            return RedirectToAction(nameof(Index));
        }

        [Permission("Pages", "Edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var page = await _pageService.GetPageByIdAsync(id);
            if (page == null)
                return NotFound();

            return View(page);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Pages", "Edit")]
        public async Task<IActionResult> Edit(int id, Page page, IFormFile? featuredImage, IFormFile? bannerImage)
        {
            if (id != page.Id)
                return NotFound();

            if (!ModelState.IsValid)
                return View(page);

            var existing = await _pageService.GetPageByIdAsync(id);
            if (existing == null)
                return NotFound();

            if (featuredImage != null)
            {
                page.FeaturedImage = await SaveImage(featuredImage, "pages");
            }
            else
            {
                page.FeaturedImage = existing.FeaturedImage;
            }

            if (bannerImage != null)
            {
                page.BannerImage = await SaveImage(bannerImage, "pages/banners");
            }
            else
            {
                page.BannerImage = existing.BannerImage;
            }

            page.UpdatedBy = User.Identity?.Name;
            await _pageService.UpdatePageAsync(page);

            TempData["Success"] = "Page updated successfully!";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Pages", "Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            await _pageService.DeletePageAsync(id);
            TempData["Success"] = "Page deleted successfully!";
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
