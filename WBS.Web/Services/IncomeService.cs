using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;

namespace WBS.Web.Services
{
    public interface IIncomeService
    {
        // Income Categories
        Task<List<IncomeCategory>> GetAllIncomeCategoriesAsync();
        Task<List<IncomeCategory>> GetActiveIncomeCategoriesAsync();
        Task<IncomeCategory?> GetIncomeCategoryByIdAsync(int id);
        Task<IncomeCategory> CreateIncomeCategoryAsync(IncomeCategory category);
        Task<IncomeCategory> UpdateIncomeCategoryAsync(IncomeCategory category);
        Task DeleteIncomeCategoryAsync(int id);

        // Other Incomes
        Task<List<OtherIncome>> GetAllOtherIncomesAsync();
        Task<OtherIncome?> GetOtherIncomeByIdAsync(int id);
        Task<OtherIncome> CreateOtherIncomeAsync(OtherIncome income);
        Task<OtherIncome> UpdateOtherIncomeAsync(OtherIncome income);
        Task DeleteOtherIncomeAsync(int id);
        Task<decimal> GetTotalOtherIncomeAsync();
        Task<decimal> GetTotalOtherIncomeByCategoryAsync(int categoryId);
        Task<List<OtherIncome>> GetOtherIncomesByCategoryAsync(int categoryId);
    }

    public class IncomeService : IIncomeService
    {
        private readonly ApplicationDbContext _context;
        private readonly IAccountService _accountService;

        public IncomeService(ApplicationDbContext context, IAccountService accountService)
        {
            _context = context;
            _accountService = accountService;
        }

        #region Income Categories
        public async Task<List<IncomeCategory>> GetAllIncomeCategoriesAsync()
        {
            return await _context.IncomeCategories
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<List<IncomeCategory>> GetActiveIncomeCategoriesAsync()
        {
            return await _context.IncomeCategories
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ThenBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<IncomeCategory?> GetIncomeCategoryByIdAsync(int id)
        {
            return await _context.IncomeCategories.FindAsync(id);
        }

        public async Task<IncomeCategory> CreateIncomeCategoryAsync(IncomeCategory category)
        {
            category.CreatedAt = DateTime.UtcNow;
            _context.IncomeCategories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<IncomeCategory> UpdateIncomeCategoryAsync(IncomeCategory category)
        {
            var existing = await _context.IncomeCategories.FindAsync(category.Id)
                ?? throw new KeyNotFoundException("Income category not found");

            _context.Entry(existing).CurrentValues.SetValues(category);
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteIncomeCategoryAsync(int id)
        {
            var category = await _context.IncomeCategories.FindAsync(id)
                ?? throw new KeyNotFoundException("Income category not found");

            _context.IncomeCategories.Remove(category);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Other Incomes
        public async Task<List<OtherIncome>> GetAllOtherIncomesAsync()
        {
            return await _context.OtherIncomes
                .Include(i => i.IncomeCategory)
                .OrderByDescending(i => i.IncomeDate)
                .ToListAsync();
        }

        public async Task<OtherIncome?> GetOtherIncomeByIdAsync(int id)
        {
            return await _context.OtherIncomes
                .Include(i => i.IncomeCategory)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<OtherIncome> CreateOtherIncomeAsync(OtherIncome income)
        {
            income.CreatedAt = DateTime.UtcNow;
            _context.OtherIncomes.Add(income);
            await _context.SaveChangesAsync();

            // Update account balance
            await _accountService.UpdateAccountBalanceAsync(
                income.AccountId,
                income.Amount,
                "Income",
                income.Description,
                "OtherIncome",
                income.Id,
                income.CreatedBy
            );

            return income;
        }

        public async Task<OtherIncome> UpdateOtherIncomeAsync(OtherIncome income)
        {
            var existing = await _context.OtherIncomes.FindAsync(income.Id)
                ?? throw new KeyNotFoundException("Other income not found");

            // If amount or account changed, need to reverse old transaction and create new one
            if (existing.Amount != income.Amount || existing.AccountId != income.AccountId)
            {
                // Reverse old income (remove money)
                await _accountService.UpdateAccountBalanceAsync(
                    existing.AccountId,
                    existing.Amount,
                    "Deleted_Income",
                    $"Reversed due to income update (ID: {existing.Id})",
                    "OtherIncome",
                    existing.Id,
                    income.CreatedBy
                );

                // Create new income
                await _accountService.UpdateAccountBalanceAsync(
                    income.AccountId,
                    income.Amount,
                    "Income",
                    income.Description,
                    "OtherIncome",
                    income.Id,
                    income.CreatedBy
                );
            }

            _context.Entry(existing).CurrentValues.SetValues(income);
            existing.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteOtherIncomeAsync(int id)
        {
            var income = await _context.OtherIncomes.FindAsync(id)
                ?? throw new KeyNotFoundException("Other income not found");

            // Reverse the income (remove money from account)
            await _accountService.UpdateAccountBalanceAsync(
                income.AccountId,
                income.Amount,
                "Deleted_Income",
                $"Income deleted (ID: {income.Id})",
                "OtherIncome",
                income.Id,
                "System"
            );

            _context.OtherIncomes.Remove(income);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetTotalOtherIncomeAsync()
        {
            return await _context.OtherIncomes
                .Where(i => i.IsActive)
                .SumAsync(i => i.Amount);
        }

        public async Task<decimal> GetTotalOtherIncomeByCategoryAsync(int categoryId)
        {
            return await _context.OtherIncomes
                .Where(i => i.IsActive && i.IncomeCategoryId == categoryId)
                .SumAsync(i => i.Amount);
        }

        public async Task<List<OtherIncome>> GetOtherIncomesByCategoryAsync(int categoryId)
        {
            return await _context.OtherIncomes
                .Include(i => i.IncomeCategory)
                .Where(i => i.IncomeCategoryId == categoryId)
                .OrderByDescending(i => i.IncomeDate)
                .ToListAsync();
        }
        #endregion
    }
}
