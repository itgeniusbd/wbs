using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DatabaseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DatabaseController> _logger;
        private readonly IConfiguration _configuration;

        public DatabaseController(ApplicationDbContext context, ILogger<DatabaseController> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FixBengaliEncoding()
        {
            try
            {
                // Fix Donation Types
                await DbScripts.BengaliTextSeeder.SeedBengaliTextAsync(_context);
                await DbScripts.BengaliTextSeeder.UpdateExistingSlidersAsync(_context);

                // Fix SDGs Bengali text
                await FixSDGBengaliText();

                TempData["Success"] = "Bengali text has been updated successfully!";
                _logger.LogInformation("Bengali encoding fixed successfully");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
                _logger.LogError(ex, "Error fixing Bengali encoding");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> FixSDGBengaliText()
        {
            try
            {
                var sdgs = await _context.SDGs.ToListAsync();

                // SDG Bengali names mapping
                var bengaliNames = new Dictionary<int, string>
                {
                    { 1, "????????? ??????" },
                    { 2, "?????? ??????" },
                    { 3, "??????????? ? ??????" },
                    { 4, "???????? ??????" },
                    { 6, "????????? ???? ? ??????????" },
                    { 7, "???????? ? ????????? ?????" },
                    { 11, "????? ??? ? ????????" },
                    { 13, "??????? ???????" },
                    { 14, "????? ???? ????" },
                    { 15, "?????? ????" }
                };

                foreach (var sdg in sdgs)
                {
                    if (bengaliNames.ContainsKey(sdg.Number))
                    {
                        sdg.NameBn = bengaliNames[sdg.Number];
                    }
                }

                await _context.SaveChangesAsync();
                
                TempData["Success"] = "SDG Bengali text updated successfully!";
                _logger.LogInformation("SDG Bengali text fixed");
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error: {ex.Message}";
                _logger.LogError(ex, "Error fixing SDG Bengali text");
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> CheckEncoding()
        {
            try
            {
                var donationTypes = await _context.DonationTypes.ToListAsync();
                var result = donationTypes.Select(dt => new
                {
                    dt.Id,
                    dt.Name,
                    NameBn = dt.NameBn ?? "NULL",
                    EncodingCheck = !string.IsNullOrEmpty(dt.NameBn) && dt.NameBn.Contains("?") ? "? Has ? marks" : "? OK"
                }).ToList();

                return Json(new { success = true, data = result });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking encoding");
                return Json(new { success = false, error = ex.Message });
            }
        }
    }
}
