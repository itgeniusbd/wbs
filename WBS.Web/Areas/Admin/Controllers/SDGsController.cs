using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class SDGsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public SDGsController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> Index()
        {
            var sdgs = await _context.SDGs
                .OrderBy(s => s.DisplayOrder)
                .ThenBy(s => s.Number)
                .ToListAsync();
            return View(sdgs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SDG sdg, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "sdgs");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    sdg.Image = $"/uploads/sdgs/{uniqueFileName}";
                }

                _context.Add(sdg);
                await _context.SaveChangesAsync();
                TempData["Success"] = "SDG created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(sdg);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sdg = await _context.SDGs.FindAsync(id);
            if (sdg == null)
            {
                return NotFound();
            }
            return View(sdg);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, SDG sdg, IFormFile? imageFile)
        {
            if (id != sdg.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (imageFile != null && imageFile.Length > 0)
                    {
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(sdg.Image))
                        {
                            var oldImagePath = Path.Combine(_environment.WebRootPath, sdg.Image.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Upload new image
                        var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "sdgs");
                        Directory.CreateDirectory(uploadsFolder);

                        var uniqueFileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await imageFile.CopyToAsync(fileStream);
                        }

                        sdg.Image = $"/uploads/sdgs/{uniqueFileName}";
                    }

                    _context.Update(sdg);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "SDG updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SDGExists(sdg.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sdg);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sdg = await _context.SDGs
                .Include(s => s.Sectors)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sdg == null)
            {
                return NotFound();
            }

            return View(sdg);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sdg = await _context.SDGs.FindAsync(id);
            if (sdg != null)
            {
                // Delete image if exists
                if (!string.IsNullOrEmpty(sdg.Image))
                {
                    var imagePath = Path.Combine(_environment.WebRootPath, sdg.Image.TrimStart('/'));
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.SDGs.Remove(sdg);
                await _context.SaveChangesAsync();
                TempData["Success"] = "SDG deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }

        private bool SDGExists(int id)
        {
            return _context.SDGs.Any(e => e.Id == id);
        }
    }
}
