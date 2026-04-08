using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class CareersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CareersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Admin/Careers
        public async Task<IActionResult> Index()
        {
            var careers = await _context.Careers
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
            return View(careers);
        }

        // GET: Admin/Careers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Careers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Career career)
        {
            // Generate slug before validation
            if (!string.IsNullOrEmpty(career.Title))
            {
                career.Slug = GenerateSlug(career.Title);
            }
            
            // Remove Slug from ModelState validation
            ModelState.Remove("Slug");
            
            if (ModelState.IsValid)
            {
                career.CreatedAt = DateTime.UtcNow;
                
                _context.Careers.Add(career);
                await _context.SaveChangesAsync();
                
                TempData["Success"] = "Career posting created successfully!";
                return RedirectToAction(nameof(Index));
            }
            
            // Log validation errors for debugging
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
            }
            
            return View(career);
        }

        // GET: Admin/Careers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var career = await _context.Careers.FindAsync(id);
            if (career == null)
                return NotFound();

            return View(career);
        }

        // POST: Admin/Careers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Career career)
        {
            if (id != career.Id)
                return NotFound();

            // Generate slug before validation
            if (!string.IsNullOrEmpty(career.Title))
            {
                career.Slug = GenerateSlug(career.Title);
            }
            
            // Remove Slug from ModelState validation
            ModelState.Remove("Slug");
            
            if (ModelState.IsValid)
            {
                try
                {
                    career.UpdatedAt = DateTime.UtcNow;
                    
                    _context.Update(career);
                    await _context.SaveChangesAsync();
                    
                    TempData["Success"] = "Career posting updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CareerExists(career.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            
            // Log validation errors for debugging
            foreach (var modelState in ModelState.Values)
            {
                foreach (var error in modelState.Errors)
                {
                    Console.WriteLine($"Validation Error: {error.ErrorMessage}");
                }
            }
            
            return View(career);
        }

        // GET: Admin/Careers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var career = await _context.Careers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (career == null)
                return NotFound();

            return View(career);
        }

        // POST: Admin/Careers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var career = await _context.Careers.FindAsync(id);
            if (career != null)
            {
                _context.Careers.Remove(career);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Career posting deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool CareerExists(int id)
        {
            return _context.Careers.Any(e => e.Id == id);
        }

        private string GenerateSlug(string title)
        {
            return title.ToLower()
                .Replace(" ", "-")
                .Replace("'", "")
                .Replace(",", "")
                .Replace(".", "")
                .Replace("!", "")
                .Replace("?", "");
        }
    }
}
