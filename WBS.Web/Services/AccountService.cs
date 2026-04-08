using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;
using Account = WBS.Web.Models.Account;

namespace WBS.Web.Services
{
    public interface IAccountService
    {
        // Account Management
        Task<List<Account>> GetAllAccountsAsync();
        Task<List<Account>> GetActiveAccountsAsync();
        Task<Account?> GetAccountByIdAsync(int id);
        Task<Account?> GetAccountByNameAsync(string accountName);
        Task<Account> CreateAccountAsync(Account account);
        Task<Account> UpdateAccountAsync(Account account);
        Task DeleteAccountAsync(int id);
        Task<Account?> GetDefaultAccountAsync();
        Task SetDefaultAccountAsync(int accountId);

        // Balance Management
        Task<decimal> GetAccountBalanceAsync(int accountId);
        Task<bool> CheckSufficientBalanceAsync(int accountId, decimal amount);
        Task UpdateAccountBalanceAsync(int accountId, decimal amount, string transactionType, string? description = null, string? referenceType = null, int? referenceId = null, string? createdBy = null);

        // Transaction History
        Task<List<AccountTransaction>> GetAccountTransactionsAsync(int accountId, DateTime? fromDate = null, DateTime? toDate = null);
        Task<List<AccountTransaction>> GetAllTransactionsAsync(DateTime? fromDate = null, DateTime? toDate = null);

        // Reports
        Task<Dictionary<string, decimal>> GetAccountSummaryAsync(int accountId);
        Task<Dictionary<string, decimal>> GetAllAccountsSummaryAsync();
    }

    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;

        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        #region Account Management
        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await _context.Accounts
                .OrderBy(a => a.DisplayOrder)
                .ThenBy(a => a.AccountName)
                .ToListAsync();
        }

        public async Task<List<Account>> GetActiveAccountsAsync()
        {
            return await _context.Accounts
                .Where(a => a.IsActive)
                .OrderBy(a => a.DisplayOrder)
                .ThenBy(a => a.AccountName)
                .ToListAsync();
        }

        public async Task<Account?> GetAccountByIdAsync(int id)
        {
            return await _context.Accounts.FindAsync(id);
        }

        public async Task<Account?> GetAccountByNameAsync(string accountName)
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountName == accountName);
        }

        public async Task<Account> CreateAccountAsync(Account account)
        {
            account.AccountCreateDate = DateTime.UtcNow;
            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();
            return account;
        }

        public async Task<Account> UpdateAccountAsync(Account account)
        {
            var existing = await _context.Accounts.FindAsync(account.Id)
                ?? throw new KeyNotFoundException("Account not found");

            existing.AccountName = account.AccountName;
            existing.AccountNameBn = account.AccountNameBn;
            existing.Description = account.Description;
            existing.DescriptionBn = account.DescriptionBn;
            existing.AccountType = account.AccountType;
            existing.AccountNumber = account.AccountNumber;
            existing.BankName = account.BankName;
            existing.BranchName = account.BranchName;
            existing.IsActive = account.IsActive;
            existing.DisplayOrder = account.DisplayOrder;
            existing.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return existing;
        }

        public async Task DeleteAccountAsync(int id)
        {
            var account = await _context.Accounts.FindAsync(id)
                ?? throw new KeyNotFoundException("Account not found");

            // Check if account has transactions
            var hasTransactions = await _context.AccountTransactions
                .AnyAsync(t => t.AccountId == id);

            if (hasTransactions)
            {
                throw new InvalidOperationException("Cannot delete account with existing transactions. Please deactivate instead.");
            }

            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
        }

        public async Task<Account?> GetDefaultAccountAsync()
        {
            return await _context.Accounts
                .FirstOrDefaultAsync(a => a.Default_Status && a.IsActive);
        }

        public async Task SetDefaultAccountAsync(int accountId)
        {
            // Remove default status from all accounts
            var allAccounts = await _context.Accounts.ToListAsync();
            foreach (var acc in allAccounts)
            {
                acc.Default_Status = false;
            }

            // Set new default account
            var account = await _context.Accounts.FindAsync(accountId)
                ?? throw new KeyNotFoundException("Account not found");
            
            account.Default_Status = true;
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Balance Management
        public async Task<decimal> GetAccountBalanceAsync(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId)
                ?? throw new KeyNotFoundException("Account not found");
            
            return account.AccountBalance;
        }

        public async Task<bool> CheckSufficientBalanceAsync(int accountId, decimal amount)
        {
            var balance = await GetAccountBalanceAsync(accountId);
            return balance >= amount;
        }

        public async Task UpdateAccountBalanceAsync(int accountId, decimal amount, string transactionType, 
            string? description = null, string? referenceType = null, int? referenceId = null, string? createdBy = null)
        {
            var account = await _context.Accounts.FindAsync(accountId)
                ?? throw new KeyNotFoundException("Account not found");

            var balanceBefore = account.AccountBalance;
            decimal balanceAfter;

            // Update balance based on transaction type
            switch (transactionType.ToLower())
            {
                case "income":
                    account.AccountBalance += amount;
                    account.Total_IN += amount;
                    account.Total_Income += amount;
                    balanceAfter = account.AccountBalance;
                    break;

                case "expense":
                    if (account.AccountBalance < amount)
                    {
                        throw new InvalidOperationException($"Insufficient balance in {account.AccountName}. Available: ?{account.AccountBalance:N2}, Required: ?{amount:N2}");
                    }
                    account.AccountBalance -= amount;
                    account.Total_OUT += amount;
                    account.Total_Expense += amount;
                    balanceAfter = account.AccountBalance;
                    break;

                case "transfer_in":
                    account.AccountBalance += amount;
                    account.Total_IN += amount;
                    balanceAfter = account.AccountBalance;
                    break;

                case "transfer_out":
                    if (account.AccountBalance < amount)
                    {
                        throw new InvalidOperationException($"Insufficient balance in {account.AccountName}. Available: ?{account.AccountBalance:N2}, Required: ?{amount:N2}");
                    }
                    account.AccountBalance -= amount;
                    account.Total_OUT += amount;
                    balanceAfter = account.AccountBalance;
                    break;

                case "deleted_income":
                    account.AccountBalance -= amount;
                    account.Total_IN -= amount;
                    account.Total_Income -= amount;
                    account.Deleted_Income += amount;
                    balanceAfter = account.AccountBalance;
                    break;

                case "deleted_expense":
                    account.AccountBalance += amount;
                    account.Total_OUT -= amount;
                    account.Total_Expense -= amount;
                    account.Deleted_Expense += amount;
                    balanceAfter = account.AccountBalance;
                    break;

                default:
                    throw new ArgumentException($"Invalid transaction type: {transactionType}");
            }

            account.UpdatedAt = DateTime.UtcNow;

            // Create transaction record
            var transaction = new AccountTransaction
            {
                AccountId = accountId,
                TransactionType = transactionType,
                Amount = amount,
                BalanceBefore = balanceBefore,
                BalanceAfter = balanceAfter,
                TransactionDate = DateTime.UtcNow,
                Description = description,
                ReferenceType = referenceType,
                ReferenceId = referenceId,
                CreatedBy = createdBy,
                CreatedAt = DateTime.UtcNow
            };

            _context.AccountTransactions.Add(transaction);
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Transaction History
        public async Task<List<AccountTransaction>> GetAccountTransactionsAsync(int accountId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.AccountTransactions
                .Include(t => t.Account)
                .Where(t => t.AccountId == accountId);

            if (fromDate.HasValue)
            {
                query = query.Where(t => t.TransactionDate.Date >= fromDate.Value.Date);
            }

            if (toDate.HasValue)
            {
                query = query.Where(t => t.TransactionDate.Date <= toDate.Value.Date);
            }

            return await query
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }

        public async Task<List<AccountTransaction>> GetAllTransactionsAsync(DateTime? fromDate = null, DateTime? toDate = null)
        {
            var query = _context.AccountTransactions
                .Include(t => t.Account)
                .AsQueryable();

            if (fromDate.HasValue)
            {
                query = query.Where(t => t.TransactionDate.Date >= fromDate.Value.Date);
            }

            if (toDate.HasValue)
            {
                query = query.Where(t => t.TransactionDate.Date <= toDate.Value.Date);
            }

            return await query
                .OrderByDescending(t => t.TransactionDate)
                .ToListAsync();
        }
        #endregion

        #region Reports
        public async Task<Dictionary<string, decimal>> GetAccountSummaryAsync(int accountId)
        {
            var account = await _context.Accounts.FindAsync(accountId)
                ?? throw new KeyNotFoundException("Account not found");

            return new Dictionary<string, decimal>
            {
                { "CurrentBalance", account.AccountBalance },
                { "TotalIncome", account.Total_Income },
                { "TotalExpense", account.Total_Expense },
                { "TotalIn", account.Total_IN },
                { "TotalOut", account.Total_OUT },
                { "DeletedIncome", account.Deleted_Income },
                { "DeletedExpense", account.Deleted_Expense }
            };
        }

        public async Task<Dictionary<string, decimal>> GetAllAccountsSummaryAsync()
        {
            var accounts = await _context.Accounts.Where(a => a.IsActive).ToListAsync();

            return new Dictionary<string, decimal>
            {
                { "TotalBalance", accounts.Sum(a => a.AccountBalance) },
                { "TotalIncome", accounts.Sum(a => a.Total_Income) },
                { "TotalExpense", accounts.Sum(a => a.Total_Expense) },
                { "TotalIn", accounts.Sum(a => a.Total_IN) },
                { "TotalOut", accounts.Sum(a => a.Total_OUT) }
            };
        }
        #endregion
    }
}
