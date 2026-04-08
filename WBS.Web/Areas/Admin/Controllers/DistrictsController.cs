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
    public class DistrictsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DistrictsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [Permission("Districts", "View")]
        public async Task<IActionResult> Index()
        {
            var districts = await _context.Districts
                .Include(d => d.Upazilas)
                .OrderBy(d => d.DisplayOrder)
                .ThenBy(d => d.Name)
                .ToListAsync();
            return View(districts);
        }

        [Permission("Districts", "Create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Permission("Districts", "Create")]
        public async Task<IActionResult> Create(District district)
        {
            if (ModelState.IsValid)
            {
                district.CreatedAt = DateTime.UtcNow;
                _context.Districts.Add(district);
                await _context.SaveChangesAsync();
                TempData["Success"] = "District created successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View(district);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var district = await _context.Districts.FindAsync(id);
            if (district == null) return NotFound();

            return View(district);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, District district)
        {
            if (id != district.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(district);
                    await _context.SaveChangesAsync();
                    TempData["Success"] = "District updated successfully!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DistrictExists(district.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(district);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var district = await _context.Districts
                .Include(d => d.Upazilas)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (district == null) return NotFound();

            return View(district);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var district = await _context.Districts.FindAsync(id);
            if (district != null)
            {
                _context.Districts.Remove(district);
                await _context.SaveChangesAsync();
                TempData["Success"] = "District deleted successfully!";
            }
            return RedirectToAction(nameof(Index));
        }

        private bool DistrictExists(int id)
        {
            return _context.Districts.Any(e => e.Id == id);
        }

        // Fix Bengali Names Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FixBengaliNames()
        {
            try
            {
                var updates = new Dictionary<string, string>
                {
                    // Dhaka Division
                    { "Dhaka", "????" },
                    { "Faridpur", "???????" },
                    { "Gazipur", "???????" },
                    { "Gopalganj", "?????????" },
                    { "Kishoreganj", "?????????" },
                    { "Madaripur", "?????????" },
                    { "Manikganj", "?????????" },
                    { "Munshiganj", "??????????" },
                    { "Narayanganj", "???????????" },
                    { "Narsingdi", "???????" },
                    { "Rajbari", "????????" },
                    { "Shariatpur", "?????????" },
                    { "Tangail", "????????" },
                    
                    // Chittagong Division
                    { "Bandarban", "?????????" },
                    { "Brahmanbaria", "????????????????" },
                    { "Chandpur", "???????" },
                    { "Chattogram", "?????????" },
                    { "Cumilla", "????????" },
                    { "Cox's Bazar", "?????????" },
                    { "Feni", "????" },
                    { "Khagrachari", "??????????" },
                    { "Lakshmipur", "??????????" },
                    { "Noakhali", "?????????" },
                    { "Rangamati", "??????????" },
                    
                    // Rajshahi Division
                    { "Bogura", "??????" },
                    { "Joypurhat", "?????????" },
                    { "Naogaon", "?????" },
                    { "Natore", "?????" },
                    { "Chapainawabganj", "??????????????" },
                    { "Pabna", "?????" },
                    { "Rajshahi", "???????" },
                    { "Sirajganj", "?????????" },
                    
                    // Khulna Division
                    { "Bagerhat", "????????" },
                    { "Chuadanga", "???????????" },
                    { "Jashore", "????" },
                    { "Jhenaidah", "???????" },
                    { "Khulna", "?????" },
                    { "Kushtia", "?????????" },
                    { "Magura", "??????" },
                    { "Meherpur", "????????" },
                    { "Narail", "??????" },
                    { "Satkhira", "?????????" },
                    
                    // Barishal Division
                    { "Barguna", "??????" },
                    { "Barishal", "??????" },
                    { "Bhola", "????" },
                    { "Jhalokati", "???????" },
                    { "Patuakhali", "??????????" },
                    { "Pirojpur", "????????" },
                    
                    // Sylhet Division
                    { "Habiganj", "???????" },
                    { "Moulvibazar", "??????????" },
                    { "Sunamganj", "?????????" },
                    { "Sylhet", "?????" },
                    
                    // Rangpur Division
                    { "Dinajpur", "????????" },
                    { "Gaibandha", "?????????" },
                    { "Kurigram", "??????????" },
                    { "Lalmonirhat", "??????????" },
                    { "Nilphamari", "?????????" },
                    { "Panchagarh", "???????" },
                    { "Rangpur", "?????" },
                    { "Thakurgaon", "?????????" },
                    
                    // Mymensingh Division
                    { "Jamalpur", "????????" },
                    { "Mymensingh", "?????????" },
                    { "Netrokona", "?????????" },
                    { "Sherpur", "??????" }
                };

                int updatedCount = 0;
                foreach (var update in updates)
                {
                    var district = await _context.Districts
                        .FirstOrDefaultAsync(d => d.Name == update.Key);
                    
                    if (district != null)
                    {
                        district.NameBn = update.Value;
                        updatedCount++;
                    }
                }

                await _context.SaveChangesAsync();
                TempData["Success"] = $"Successfully updated {updatedCount} district Bengali names!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error updating Bengali names: {ex.Message}";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
