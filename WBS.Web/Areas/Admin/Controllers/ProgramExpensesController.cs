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
    public class ProgramExpensesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProgramExpensesController> _logger;
        private readonly IAccountService _accountService;

        public ProgramExpensesController(
            ApplicationDbContext context,
            ILogger<ProgramExpensesController> logger,
            IAccountService accountService)
        {
            _context = context;
            _logger = logger;
            _accountService = accountService;
        }

        // GET: Admin/ProgramExpenses
        [Permission("Program Expenses", "View")]
        public async Task<IActionResult> Index(int? sdgId, int? programId, int? projectId, DateTime? fromDate, DateTime? toDate)
        {
            try
            {
                var query = _context.ProgramExpenses
                    .Include(e => e.SDG)
                    .Include(e => e.Program)
                    .Include(e => e.Project)
                    .Include(e => e.Account)
                    .Where(e => e.IsActive)
                    .AsQueryable();

                // Apply filters
                if (sdgId.HasValue)
                    query = query.Where(e => e.SDGId == sdgId.Value);

                if (programId.HasValue)
                    query = query.Where(e => e.ProgramId == programId.Value);

                if (projectId.HasValue)
                    query = query.Where(e => e.ProjectId == projectId.Value);

                if (fromDate.HasValue)
                    query = query.Where(e => e.ExpenseDate >= fromDate.Value);

                if (toDate.HasValue)
                    query = query.Where(e => e.ExpenseDate <= toDate.Value);

                var expenses = await query
                    .OrderByDescending(e => e.ExpenseDate)
                    .ToListAsync();

                // Calculate total
                ViewBag.TotalExpense = expenses.Sum(e => e.Amount);

                // Load filter dropdowns
                ViewBag.SDGs = await _context.SDGs.Where(s => s.IsActive).OrderBy(s => s.Name).ToListAsync();
                ViewBag.Programs = await _context.SDGPrograms.Where(p => p.IsActive).OrderBy(p => p.Title).ToListAsync();
                ViewBag.Projects = await _context.SDGProjects.Where(p => p.IsActive).OrderBy(p => p.Title).ToListAsync();
                ViewBag.Accounts = await _context.Accounts.Where(a => a.IsActive).OrderBy(a => a.AccountName).ToListAsync();

                ViewBag.SelectedSdgId = sdgId;
                ViewBag.SelectedProgramId = programId;
                ViewBag.SelectedProjectId = projectId;
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;

                return View(expenses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading program expenses");
                TempData["Error"] = "Error loading program expenses.";
                
                // Ensure ViewBag has empty lists to avoid null reference
                ViewBag.SDGs = new List<SDG>();
                ViewBag.Programs = new List<SDGProgram>();
                ViewBag.Projects = new List<SDGProject>();
                ViewBag.Accounts = new List<Account>();
                ViewBag.TotalExpense = 0m;
                
                return View(new List<ProgramExpense>());
            }
        }

        // GET: Admin/ProgramExpenses/Create
        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.SDGs = await _context.SDGs.Where(s => s.IsActive).OrderBy(s => s.Name).ToListAsync();
                ViewBag.Accounts = await _context.Accounts.Where(a => a.IsActive).OrderBy(a => a.AccountName).ToListAsync();
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading create form");
                TempData["Error"] = "Error loading form.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/ProgramExpenses/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProgramExpense expense)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    expense.CreatedBy = User.Identity?.Name;
                    expense.CreatedAt = DateTime.UtcNow;

                    _context.ProgramExpenses.Add(expense);
                    await _context.SaveChangesAsync();

                    // Update account balance
                    await _accountService.UpdateAccountBalanceAsync(
                        accountId: expense.AccountId,
                        amount: expense.Amount,
                        transactionType: "Expense",
                        description: $"Program Expense - {expense.Details}",
                        referenceType: "ProgramExpense",
                        referenceId: expense.Id
                    );

                    TempData["Success"] = "Program expense added successfully.";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.SDGs = await _context.SDGs.Where(s => s.IsActive).OrderBy(s => s.Name).ToListAsync();
                ViewBag.Accounts = await _context.Accounts.Where(a => a.IsActive).OrderBy(a => a.AccountName).ToListAsync();
                return View(expense);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating program expense");
                TempData["Error"] = "Error creating program expense.";
                
                ViewBag.SDGs = await _context.SDGs.Where(s => s.IsActive).OrderBy(s => s.Name).ToListAsync();
                ViewBag.Accounts = await _context.Accounts.Where(a => a.IsActive).OrderBy(a => a.AccountName).ToListAsync();
                return View(expense);
            }
        }

        // GET: Admin/ProgramExpenses/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var expense = await _context.ProgramExpenses
                    .Include(e => e.SDG)
                    .Include(e => e.Program)
                    .Include(e => e.Project)
                    .FirstOrDefaultAsync(e => e.Id == id);

                if (expense == null)
                {
                    TempData["Error"] = "Program expense not found.";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.SDGs = await _context.SDGs.Where(s => s.IsActive).OrderBy(s => s.Name).ToListAsync();
                ViewBag.Programs = await _context.SDGPrograms
                    .Where(p => p.IsActive && p.SDGId == expense.SDGId)
                    .OrderBy(p => p.Title)
                    .ToListAsync();
                ViewBag.Projects = await _context.SDGProjects
                    .Where(p => p.IsActive && p.SDGProgramId == expense.ProgramId)
                    .OrderBy(p => p.Title)
                    .ToListAsync();
                ViewBag.Accounts = await _context.Accounts.Where(a => a.IsActive).OrderBy(a => a.AccountName).ToListAsync();

                return View(expense);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading edit form");
                TempData["Error"] = "Error loading expense.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/ProgramExpenses/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProgramExpense expense)
        {
            if (id != expense.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var existing = await _context.ProgramExpenses.FindAsync(id);
                    if (existing == null)
                    {
                        TempData["Error"] = "Program expense not found.";
                        return RedirectToAction(nameof(Index));
                    }

                    var oldAmount = existing.Amount;
                    var oldAccountId = existing.AccountId;

                    existing.SDGId = expense.SDGId;
                    existing.ProgramId = expense.ProgramId;
                    existing.ProjectId = expense.ProjectId;
                    existing.Amount = expense.Amount;
                    existing.AccountId = expense.AccountId;
                    existing.ExpenseDate = expense.ExpenseDate;
                    existing.Details = expense.Details;
                    existing.UpdatedAt = DateTime.UtcNow;

                    await _context.SaveChangesAsync();

                    // Reverse old transaction
                    if (oldAccountId > 0)
                    {
                        await _accountService.UpdateAccountBalanceAsync(
                            accountId: oldAccountId,
                            amount: oldAmount,
                            transactionType: "Income",
                            description: $"Reversed Program Expense - {expense.Details}",
                            referenceType: "ProgramExpense",
                            referenceId: expense.Id
                        );
                    }

                    // Create new transaction
                    await _accountService.UpdateAccountBalanceAsync(
                        accountId: expense.AccountId,
                        amount: expense.Amount,
                        transactionType: "Expense",
                        description: $"Program Expense - {expense.Details}",
                        referenceType: "ProgramExpense",
                        referenceId: expense.Id
                    );

                    TempData["Success"] = "Program expense updated successfully.";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.SDGs = await _context.SDGs.Where(s => s.IsActive).OrderBy(s => s.Name).ToListAsync();
                ViewBag.Programs = await _context.SDGPrograms
                    .Where(p => p.IsActive && p.SDGId == expense.SDGId)
                    .OrderBy(p => p.Title)
                    .ToListAsync();
                ViewBag.Projects = await _context.SDGProjects
                    .Where(p => p.IsActive && p.SDGProgramId == expense.ProgramId)
                    .OrderBy(p => p.Title)
                    .ToListAsync();
                ViewBag.Accounts = await _context.Accounts.Where(a => a.IsActive).OrderBy(a => a.AccountName).ToListAsync();

                return View(expense);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating program expense");
                TempData["Error"] = "Error updating program expense.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Admin/ProgramExpenses/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var expense = await _context.ProgramExpenses.FindAsync(id);
                if (expense == null)
                {
                    return Json(new { success = false, message = "Program expense not found." });
                }

                // Soft delete
                expense.IsActive = false;
                await _context.SaveChangesAsync();

                // Reverse transaction
                await _accountService.UpdateAccountBalanceAsync(
                    accountId: expense.AccountId,
                    amount: expense.Amount,
                    transactionType: "Income",
                    description: $"Deleted Program Expense - {expense.Details}",
                    referenceType: "ProgramExpense",
                    referenceId: expense.Id
                );

                return Json(new { success = true, message = "Program expense deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting program expense");
                return Json(new { success = false, message = "Error deleting program expense." });
            }
        }

        // API endpoint to get programs by SDG
        [HttpGet]
        public async Task<IActionResult> GetProgramsBySDG(int sdgId)
        {
            try
            {
                var programs = await _context.SDGPrograms
                    .Where(p => p.IsActive && p.SDGId == sdgId)
                    .OrderBy(p => p.Title)
                    .Select(p => new { id = p.Id, title = p.Title })
                    .ToListAsync();

                return Json(programs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading programs");
                return Json(new List<object>());
            }
        }

        // API endpoint to get projects by program
        [HttpGet]
        public async Task<IActionResult> GetProjectsByProgram(int programId)
        {
            try
            {
                var projects = await _context.SDGProjects
                    .Where(p => p.IsActive && p.SDGProgramId == programId)
                    .OrderBy(p => p.Title)
                    .Select(p => new { id = p.Id, title = p.Title })
                    .ToListAsync();

                return Json(projects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading projects");
                return Json(new List<object>());
            }
        }
    }
}
