using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;

namespace WBS.Web.Services
{
    public interface IExpenseService
    {
        // Expense Categories
        Task<List<ExpenseCategory>> GetAllExpenseCategoriesAsync();
        Task<List<ExpenseCategory>> GetActiveExpenseCategoriesAsync();
        Task<ExpenseCategory?> GetExpenseCategoryByIdAsync(int id);
        Task<ExpenseCategory> CreateExpenseCategoryAsync(ExpenseCategory category);
        Task<ExpenseCategory> UpdateExpenseCategoryAsync(ExpenseCategory category);
        Task DeleteExpenseCategoryAsync(int id);

        // General Expenses
        Task<List<GeneralExpense>> GetAllGeneralExpensesAsync();
        Task<GeneralExpense?> GetGeneralExpenseByIdAsync(int id);
        Task<GeneralExpense> CreateGeneralExpenseAsync(GeneralExpense expense);
        Task<GeneralExpense> UpdateGeneralExpenseAsync(GeneralExpense expense);
        Task DeleteGeneralExpenseAsync(int id);
        Task<decimal> GetTotalGeneralExpenseAsync();
        Task<decimal> GetTotalGeneralExpenseByCategoryAsync(int categoryId);
        Task<List<GeneralExpense>> GetGeneralExpensesByCategoryAsync(int categoryId);
    }

    public class ExpenseService : IExpenseService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountService _accountService;

        public ExpenseService(ApplicationDbContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        #region Expense Categories
        public async Task<List<ExpenseCategory>> GetAllExpenseCategoriesAsync()
        {
            return await _context.ExpenseCategories
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<List<ExpenseCategory>> GetActiveExpenseCategoriesAsync()
        {
            return await _context.ExpenseCategories
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<ExpenseCategory?> GetExpenseCategoryByIdAsync(int id)
        {
            return await _context.ExpenseCategories.FindAsync(id);
        }

        public async Task<ExpenseCategory> CreateExpenseCategoryAsync(ExpenseCategory category)
        {
            category.CreatedAt = DateTime.UtcNow;
            _context.ExpenseCategories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<ExpenseCategory> UpdateExpenseCategoryAsync(ExpenseCategory category)
        {
            var existing = await _context.ExpenseCategories.FindAsync(category.Id)
                ?? throw new KeyNotFoundException("Expense category not found");

            _context.Entry(existing).CurrentValues.SetValues(category);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteExpenseCategoryAsync(int id)
        {
            var category = await _context.ExpenseCategories.FindAsync(id)
                ?? throw new KeyNotFoundException("Expense category not found");

            _context.ExpenseCategories.Remove(category);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region General Expenses
        public async Task<List<GeneralExpense>> GetAllGeneralExpensesAsync()
        {
            return await _context.GeneralExpenses
                .Include(e => e.ExpenseCategory)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }

        public async Task<GeneralExpense?> GetGeneralExpenseByIdAsync(int id)
        {
            return await _context.GeneralExpenses
                .Include(e => e.ExpenseCategory)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<GeneralExpense> CreateGeneralExpenseAsync(GeneralExpense expense)
        {
            // Check if account has sufficient balance
            var hasSufficientBalance = await _accountService.CheckSufficientBalanceAsync(expense.AccountId, expense.Amount);
            if (!hasSufficientBalance)
            {
                var accountBalance = await _accountService.GetAccountBalanceAsync(expense.AccountId);
                var account = await _context.Accounts.FindAsync(expense.AccountId);
                throw new InvalidOperationException($"Insufficient balance in {account?.AccountName}. Available: ?{accountBalance:N2}, Required: ?{expense.Amount:N2}");
            }

            expense.CreatedAt = DateTime.UtcNow;
            _context.GeneralExpenses.Add(expense);
            await _context.SaveChangesAsync();

            // Update account balance
            await _accountService.UpdateAccountBalanceAsync(
                expense.AccountId,
                expense.Amount,
                "Expense",
                expense.Description,
                "GeneralExpense",
                expense.Id,
                expense.CreatedBy
            );

            return expense;
        }

        public async Task<GeneralExpense> UpdateGeneralExpenseAsync(GeneralExpense expense)
        {
            var existing = await _context.GeneralExpenses.FindAsync(expense.Id)
                ?? throw new KeyNotFoundException("General expense not found");

            // If amount or account changed, need to reverse old transaction and create new one
            if (existing.Amount != expense.Amount || existing.AccountId != expense.AccountId)
            {
                // Reverse old expense (add money back)
                await _accountService.UpdateAccountBalanceAsync(
                    existing.AccountId,
                    existing.Amount,
                    "Deleted_Expense",
                    $"Reversed due to expense update (ID: {existing.Id})",
                    "GeneralExpense",
                    existing.Id,
                    expense.CreatedBy
                );

                // Check new account balance
                var hasSufficientBalance = await _accountService.CheckSufficientBalanceAsync(expense.AccountId, expense.Amount);
                if (!hasSufficientBalance)
                {
                    var accountBalance = await _accountService.GetAccountBalanceAsync(expense.AccountId);
                    var account = await _context.Accounts.FindAsync(expense.AccountId);
                    throw new InvalidOperationException($"Insufficient balance in {account?.AccountName}. Available: ?{accountBalance:N2}, Required: ?{expense.Amount:N2}");
                }

                // Create new expense
                await _accountService.UpdateAccountBalanceAsync(
                    expense.AccountId,
                    expense.Amount,
                    "Expense",
                    expense.Description,
                    "GeneralExpense",
                    expense.Id,
                    expense.CreatedBy
                );
            }

            _context.Entry(existing).CurrentValues.SetValues(expense);
            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteGeneralExpenseAsync(int id)
        {
            var expense = await _context.GeneralExpenses.FindAsync(id)
                ?? throw new KeyNotFoundException("General expense not found");

            // Reverse the expense (add money back to account)
            await _accountService.UpdateAccountBalanceAsync(
                expense.AccountId,
                expense.Amount,
                "Deleted_Expense",
                $"Expense deleted (ID: {expense.Id})",
                "GeneralExpense",
                expense.Id,
                "System"
            );

            _context.GeneralExpenses.Remove(expense);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetTotalGeneralExpenseAsync()
        {
            return await _context.GeneralExpenses
                .Where(e => e.IsActive)
                .SumAsync(e => e.Amount);
        }

        public async Task<decimal> GetTotalGeneralExpenseByCategoryAsync(int categoryId)
        {
            return await _context.GeneralExpenses
                .Where(e => e.IsActive && e.ExpenseCategoryId == categoryId)
                .SumAsync(e => e.Amount);
        }

        public async Task<List<GeneralExpense>> GetGeneralExpensesByCategoryAsync(int categoryId)
        {
            return await _context.GeneralExpenses
                .Include(e => e.ExpenseCategory)
                .Where(e => e.ExpenseCategoryId == categoryId)
                .OrderByDescending(e => e.ExpenseDate)
                .ToListAsync();
        }
        #endregion
    }
}
