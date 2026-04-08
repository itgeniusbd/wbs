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
    public class GeneralExpensesController : Controller
    {
        private readonly IExpenseService _expenseService;
        private readonly ApplicationDbContext _context;

        public GeneralExpensesController(IExpenseService expenseService, ApplicationDbContext context)
        {
            _expenseService = expenseService;
            _context = context;
        }

        private bool IsBangla()
        {
            var currentCulture = HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.TwoLetterISOLanguageName ?? "en";
            return currentCulture == "bn";
        }

        // GET: Admin/GeneralExpenses - Main Page with everything
        [Permission("General Expenses", "View")]
        public async Task<IActionResult> Index(int? categoryId, DateTime? fromDate, DateTime? toDate)
        {
            var categories = await _expenseService.GetAllExpenseCategoriesAsync();
            var expenses = await _expenseService.GetAllGeneralExpensesAsync();
            var accounts = await _context.Accounts
                .Where(a => a.IsActive)
                .OrderBy(a => a.DisplayOrder)
                .ToListAsync();

            // Apply filters
            if (categoryId.HasValue && categoryId.Value > 0)
            {
                expenses = expenses.Where(e => e.ExpenseCategoryId == categoryId.Value).ToList();
            }

            if (fromDate.HasValue)
            {
                expenses = expenses.Where(e => e.ExpenseDate.Date >= fromDate.Value.Date).ToList();
            }

            if (toDate.HasValue)
            {
                expenses = expenses.Where(e => e.ExpenseDate.Date <= toDate.Value.Date).ToList();
            }

            ViewBag.Categories = categories;
            ViewBag.Accounts = accounts;
            ViewBag.SelectedCategoryId = categoryId;
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            return View(expenses);
        }

        // POST: Create Category (AJAX)
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] ExpenseCategory category)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(category.Name))
                {
                    return Json(new { success = false, message = IsBangla() ? "??? ????????" : "Name is required" });
                }

                await _expenseService.CreateExpenseCategoryAsync(category);
                return Json(new { success = true, message = IsBangla() ? "????????? ??????? ???? ??????!" : "Category created successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Update Category (AJAX)
        [HttpPost]
        public async Task<IActionResult> UpdateCategory([FromBody] ExpenseCategory category)
        {
            try
            {
                await _expenseService.UpdateExpenseCategoryAsync(category);
                return Json(new { success = true, message = IsBangla() ? "????????? ????? ??????!" : "Category updated successfully!" });
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
                await _expenseService.DeleteExpenseCategoryAsync(id);
                return Json(new { success = true, message = IsBangla() ? "????????? ???? ???? ??????!" : "Category deleted successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Create Expense (AJAX)
        [HttpPost]
        public async Task<IActionResult> CreateExpense([FromForm] GeneralExpense expense)
        {
            try
            {
                expense.CreatedBy = User.Identity?.Name ?? "Admin";
                await _expenseService.CreateGeneralExpenseAsync(expense);
                return Json(new { success = true, message = IsBangla() ? "????? ??????? ????? ??????!" : "Expense added successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Get Expense by ID (AJAX)
        [HttpGet]
        public async Task<IActionResult> GetExpense(int id)
        {
            try
            {
                var expense = await _expenseService.GetGeneralExpenseByIdAsync(id);
                if (expense == null)
                {
                    return Json(new { success = false, message = IsBangla() ? "????? ?????? ??????" : "Expense not found" });
                }
                return Json(new { success = true, data = expense });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Update Expense (AJAX)
        [HttpPost]
        public async Task<IActionResult> UpdateExpense([FromForm] GeneralExpense expense)
        {
            try
            {
                await _expenseService.UpdateGeneralExpenseAsync(expense);
                return Json(new { success = true, message = IsBangla() ? "????? ????? ??????!" : "Expense updated successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Delete Expense (AJAX)
        [HttpPost]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            try
            {
                await _expenseService.DeleteGeneralExpenseAsync(id);
                return Json(new { success = true, message = IsBangla() ? "????? ???? ???? ??????!" : "Expense deleted successfully!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
