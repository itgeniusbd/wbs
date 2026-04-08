using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using WBS.Web.Models;
using WBS.Web.Services;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Attributes;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OtherIncomesController : Controller
    {
        private readonly IIncomeService _incomeService;
        private readonly ApplicationDbContext _context;

        public OtherIncomesController(IIncomeService incomeService, ApplicationDbContext context)
        {
            _incomeService = incomeService;
            _context = context;
        }

        private bool IsBangla()
        {
            var currentCulture = HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.TwoLetterISOLanguageName ?? "en";
            return currentCulture == "bn";
        }

        // GET: Admin/OtherIncomes - Main Page with everything
        [Permission("Other Incomes", "View")]
        public async Task<IActionResult> Index(int? categoryId, DateTime? fromDate, DateTime? toDate)
        {
            var categories = await _incomeService.GetAllIncomeCategoriesAsync();
            var incomes = await _incomeService.GetAllOtherIncomesAsync();
            var accounts = await _context.Accounts
                .Where(a => a.IsActive)
                .OrderBy(a => a.DisplayOrder)
                .ThenBy(a => a.AccountName)
                .ToListAsync();

            // Apply filters
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                incomes = incomes.Where(i => i.IncomeCategoryId == categoryId.Value).ToList();
            }

            if (fromDate.HasValue)
            {
                incomes = incomes.Where(i => i.IncomeDate.Date >= fromDate.Value.Date).ToList();
            }

            if (toDate.HasValue)
            {
                incomes = incomes.Where(i => i.IncomeDate.Date <= toDate.Value.Date).ToList();
            }

            ViewBag.Categories = categories;
            ViewBag.Accounts = accounts;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            return View(incomes);
        }

        // POST: Create Category (AJAX)
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] IncomeCategory category)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(category.Name))
                {
                    return Json(new { success = false, message = IsBangla() ? "নাম প্রয়োজন" : "Name is required" });
                }

                await _incomeService.CreateIncomeCategoryAsync(category);
                return Json(new { success = true, message = IsBangla() ? "ক্যাটাগরি সফলভাবে তৈরি হয়েছে!" : "Category created successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Update Category (AJAX)
        [HttpPost]
        public async Task<IActionResult> UpdateCategory([FromBody] IncomeCategory category)
        {
            try
            {
                await _incomeService.UpdateIncomeCategoryAsync(category);
                return Json(new { success = true, message = IsBangla() ? "ক্যাটাগরি আপডেট হয়েছে!" : "Category updated successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Delete Category (AJAX)
        [HttpPost]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _incomeService.DeleteIncomeCategoryAsync(id);
                return Json(new { success = true, message = IsBangla() ? "ক্যাটাগরি মুছে ফেলা হয়েছে!" : "Category deleted successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Create Income (AJAX)
        [HttpPost]
        public async Task<IActionResult> CreateIncome([FromForm] OtherIncome income)
        {
            try
            {
                income.CreatedBy = User.Identity?.Name ?? "Admin";
                await _incomeService.CreateOtherIncomeAsync(income);
                return Json(new { success = true, message = IsBangla() ? "আয় সফলভাবে যুক্ত হয়েছে!" : "Income added successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Get Income by ID (AJAX)
        [HttpGet]
        public async Task<IActionResult> GetIncome(int id)
        {
            try
            {
                var income = await _incomeService.GetOtherIncomeByIdAsync(id);
                if (income == null)
                {
                    return Json(new { success = false, message = IsBangla() ? "আয় পাওয়া যায়নি" : "Income not found" });
                }
                return Json(new { success = true, data = income });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Update Income (AJAX)
        [HttpPost]
        public async Task<IActionResult> UpdateIncome([FromForm] OtherIncome income)
        {
            try
            {
                await _incomeService.UpdateOtherIncomeAsync(income);
                return Json(new { success = true, message = IsBangla() ? "আয় আপডেট হয়েছে!" : "Income updated successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Delete Income (AJAX)
        [HttpPost]
        public async Task<IActionResult> DeleteIncome(int id)
        {
            try
            {
                await _incomeService.DeleteOtherIncomeAsync(id);
                return Json(new { success = true, message = IsBangla() ? "আয় মুছে ফেলা হয়েছে!" : "Income deleted successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
