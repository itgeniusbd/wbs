using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;
using WBS.Web.Attributes;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class UpazilasController : Controller
    {
        private readonly ApplicationDbContext _context;

        public UpazilasController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Permission("Upazilas", "View")]
        public async Task<IActionResult> Index(int? districtId)
        {
            // Include Upazilas when loading Districts so the count is correct
            ViewBag.Districts = await _context.Districts
                .Include(d => d.Upazilas)
                .OrderBy(d => d.Name)
                .ToListAsync();
            
            var query = _context.Upazilas.Include(u => u.District).AsQueryable();
            
            if (districtId.HasValue)
            {
                query = query.Where(u => u.DistrictId == districtId.Value);
                ViewBag.SelectedDistrictId = districtId.Value;
            }
            
            var upazilas = await query
                .OrderBy(u => u.District.Name)
                .ThenBy(u => u.DisplayOrder)
                .ThenBy(u => u.Name)
                .ToListAsync();
            
            return View(upazilas);
        }

        [Permission("Upazilas", "Create")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Districts = new SelectList(await _context.Districts.OrderBy(d => d.Name).ToListAsync(), "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Upazila upazila)
        {
            // Remove District navigation property from ModelState validation
            ModelState.Remove("District");
            
            // Check for duplicate upazila in the same district
            var existingUpazila = await _context.Upazilas
                .FirstOrDefaultAsync(u => u.Name == upazila.Name && u.DistrictId == upazila.DistrictId);
            
            if (existingUpazila != null)
            {
                ModelState.AddModelError("Name", "?? ?????? ???????? ?? ?????? ????????? / This upazila already exists in this district.");
                ViewBag.Districts = new SelectList(await _context.Districts.OrderBy(d => d.Name).ToListAsync(), "Id", "Name", upazila.DistrictId);
                return View(upazila);
            }

            if (ModelState.IsValid)
            {
                upazila.CreatedAt = DateTime.UtcNow;
                _context.Upazilas.Add(upazila);
                
                try
                {
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Upazila created successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException ex)
                {
                    // Handle database errors
                    if (ex.InnerException?.Message.Contains("duplicate") == true || 
                        ex.InnerException?.Message.Contains("unique") == true)
                    {
                        ModelState.AddModelError("", "?? ?????? ???????? ????????? / This upazila already exists.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                    }
                }
            }
            
            ViewBag.Districts = new SelectList(await _context.Districts.OrderBy(d => d.Name).ToListAsync(), "Id", "Name", upazila.DistrictId);
            return View(upazila);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var upazila = await _context.Upazilas.FindAsync(id);
            if (upazila == null) return NotFound();

            ViewBag.Districts = new SelectList(await _context.Districts.OrderBy(d => d.Name).ToListAsync(), "Id", "Name", upazila.DistrictId);
            return View(upazila);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Upazila upazila)
        {
            if (id != upazila.Id) return NotFound();

            // Check for duplicate upazila in the same district (excluding current upazila)
            var existingUpazila = await _context.Upazilas
                .FirstOrDefaultAsync(u => u.Name == upazila.Name && 
                                         u.DistrictId == upazila.DistrictId && 
                                         u.Id != id);
            
            if (existingUpazila != null)
            {
                ModelState.AddModelError("Name", "?? ?????? ???????? ?? ?????? ????????? / This upazila already exists in this district.");
                ViewBag.Districts = new SelectList(await _context.Districts.OrderBy(d => d.Name).ToListAsync(), "Id", "Name", upazila.DistrictId);
                return View(upazila);
            }

            // Remove DistrictId validation if it's causing issues
            ModelState.Remove("District");

            if (ModelState.IsValid)
            {
                try
                {
                    // Make sure we preserve the created date
                    var existingRecord = await _context.Upazilas.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                    if (existingRecord != null)
                    {
                        upazila.CreatedAt = existingRecord.CreatedAt;
                    }
                    
                    _context.Update(upazila);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "Upazila updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UpazilaExists(upazila.Id))
                        return NotFound();
                    else
                        throw;
                }
                catch (DbUpdateException ex)
                {
                    // Handle database errors
                    if (ex.InnerException?.Message.Contains("duplicate") == true || 
                        ex.InnerException?.Message.Contains("unique") == true)
                    {
                        ModelState.AddModelError("", "?? ?????? ???????? ????????? / This upazila already exists.");
                    }
                    else
                    {
                        ModelState.AddModelError("", "An error occurred while saving. Please try again.");
                    }
                }
            }
            
            ViewBag.Districts = new SelectList(await _context.Districts.OrderBy(d => d.Name).ToListAsync(), "Id", "Name", upazila.DistrictId);
            return View(upazila);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var upazila = await _context.Upazilas
                .Include(u => u.District)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (upazila == null) return NotFound();

            return View(upazila);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var upazila = await _context.Upazilas.FindAsync(id);
            if (upazila != null)
            {
                _context.Upazilas.Remove(upazila);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Upazila deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool UpazilaExists(int id)
        {
            return _context.Upazilas.Any(e => e.Id == id);
        }
    }
}
