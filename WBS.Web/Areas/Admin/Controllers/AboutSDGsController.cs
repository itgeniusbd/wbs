using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.Attributes;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AboutSDGsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public AboutSDGsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Admin/AboutSDGs
        [Permission("About SDGs", "View")]
        public async Task<IActionResult> Index()
        {
            var aboutSDGs = await _context.AboutSDGs
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();

            return View(aboutSDGs);
        }

        // GET: Admin/AboutSDGs/Create
        [Permission("About SDGs", "Create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/AboutSDGs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("About SDGs", "Create")]
        public async Task<IActionResult> Create(AboutSDG aboutSDG, IFormFile? featuredImage)
        {
            // Remove FeaturedImage from ModelState validation since it's handled separately
            ModelState.Remove("FeaturedImage");

            if (featuredImage == null)
            {
                ModelState.AddModelError("featuredImage", "Featured image is required.");
            }

            if (ModelState.IsValid && featuredImage != null)
            {
                // Upload featured image
                aboutSDG.FeaturedImage = await SaveImage(featuredImage, "sdgs");

                aboutSDG.CreatedAt = DateTime.UtcNow;
                _context.AboutSDGs.Add(aboutSDG);
                await _context.SaveChangesAsync();

                TempData["Success"] = "About SDGs content created successfully!";
                return RedirectToAction(nameof(Index));
            }

            return View(aboutSDG);
        }

        // GET: Admin/AboutSDGs/Edit/5
        [Permission("About SDGs", "Edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var aboutSDG = await _context.AboutSDGs.FindAsync(id);
            if (aboutSDG == null)
                return NotFound();

            return View(aboutSDG);
        }

        // POST: Admin/AboutSDGs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("About SDGs", "Edit")]
        public async Task<IActionResult> Edit(int id, AboutSDG aboutSDG, IFormFile? featuredImage)
        {
            if (id != aboutSDG.Id)
                return NotFound();

            // Remove FeaturedImage from ModelState validation since it's handled separately
            ModelState.Remove("FeaturedImage");

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = await _context.AboutSDGs.FindAsync(id);
                    if (existing == null)
                        return NotFound();

                    // Upload new featured image if provided
                    if (featuredImage != null)
                    {
                        aboutSDG.FeaturedImage = await SaveImage(featuredImage, "sdgs");
                    }
                    else
                    {
                        aboutSDG.FeaturedImage = existing.FeaturedImage;
                    }

                    aboutSDG.CreatedAt = existing.CreatedAt;
                    aboutSDG.UpdatedAt = DateTime.UtcNow;
                    _context.Entry(existing).CurrentValues.SetValues(aboutSDG);
                    await _context.SaveChangesAsync();

                    TempData["Success"] = "About SDGs content updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AboutSDGExists(aboutSDG.Id))
                        return NotFound();
                    throw;
                }
            }

            return View(aboutSDG);
        }

        // POST: Admin/AboutSDGs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var aboutSDG = await _context.AboutSDGs.FindAsync(id);
            if (aboutSDG != null)
            {
                _context.AboutSDGs.Remove(aboutSDG);
                await _context.SaveChangesAsync();
                TempData["Success"] = "About SDGs content deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Admin/AboutSDGs/ToggleActive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleActive(int id)
        {
            var aboutSDG = await _context.AboutSDGs.FindAsync(id);
            if (aboutSDG != null)
            {
                aboutSDG.IsActive = !aboutSDG.IsActive;
                aboutSDG.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
                TempData["Success"] = $"About SDGs content {(aboutSDG.IsActive ? "activated" : "deactivated")} successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool AboutSDGExists(int id)
        {
            return _context.AboutSDGs.Any(e => e.Id == id);
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
