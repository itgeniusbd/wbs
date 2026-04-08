using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Localization;
using WBS.Web.Models;
using WBS.Web.Services;
using WBS.Web.Attributes;
using Microsoft.Extensions.Logging;

namespace WBS.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class AccountsController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(IAccountService accountService, ILogger<AccountsController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        private bool IsBangla()
        {
            var currentCulture = HttpContext.Features.Get<IRequestCultureFeature>()?.RequestCulture.Culture.TwoLetterISOLanguageName ?? "en";
            return currentCulture == "bn";
        }

        // GET: Admin/Accounts
        [Permission("Accounts", "View")]
        public async Task<IActionResult> Index()
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            return View(accounts);
        }

        // POST: Create Account (AJAX)
        [HttpPost]
        [Permission("Accounts", "Create")]
        public async Task<IActionResult> CreateAccount([FromBody] Account account)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(account.AccountName))
                {
                    return Json(new { success = false, message = IsBangla() ? "????????? ??? ????????" : "Account name is required" });
                }

                account.CreatedBy = User.Identity?.Name ?? "Admin";
                await _accountService.CreateAccountAsync(account);
                return Json(new { success = true, message = IsBangla() ? "??????? ??????? ???? ??????!" : "Account created successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating account");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Get Account by ID (AJAX)
        [HttpGet]
        [Permission("Accounts", "View")]
        public async Task<IActionResult> GetAccount(int id)
        {
            try
            {
                var account = await _accountService.GetAccountByIdAsync(id);
                if (account == null)
                {
                    return Json(new { success = false, message = IsBangla() ? "একাউন্ট পাওয়া যায়নি" : "Account not found" });
                }
                return Json(new { success = true, data = account });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching account details");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Update Account (AJAX)
        [HttpPost]
        [Permission("Accounts", "Edit")]
        public async Task<IActionResult> UpdateAccount([FromBody] Account account)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(account.AccountName))
                {
                    return Json(new { success = false, message = IsBangla() ? "একাউন্টের নাম প্রয়োজন" : "Account name is required" });
                }

                account.UpdatedAt = DateTime.UtcNow;
                await _accountService.UpdateAccountAsync(account);
                return Json(new { success = true, message = IsBangla() ? "একাউন্ট আপডেট সফল হয়েছে!" : "Account updated successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating account");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Set Default Account (AJAX)
        [HttpPost]
        [Permission("Accounts", "Edit")]
        public async Task<IActionResult> SetDefaultAccount(int id)
        {
            try
            {
                await _accountService.SetDefaultAccountAsync(id);
                return Json(new { success = true, message = IsBangla() ? "ডিফল্ট একাউন্ট সেট সফল হয়েছে!" : "Default account set successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while setting default account");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Delete Account (AJAX)
        [HttpPost]
        [Permission("Accounts", "Delete")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                await _accountService.DeleteAccountAsync(id);
                return Json(new { success = true, message = IsBangla() ? "একাউন্ট মুছে ফেলা সফল হয়েছে!" : "Account deleted successfully!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting account");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Account Transactions
        [Permission("Accounts", "View")]
        public async Task<IActionResult> Transactions(int? accountId, DateTime? fromDate, DateTime? toDate)
        {
            var accounts = await _accountService.GetAllAccountsAsync();
            
            List<AccountTransaction> transactions;
            if (accountId.HasValue && accountId.Value > 0)
            {
                transactions = await _accountService.GetAccountTransactionsAsync(accountId.Value, fromDate, toDate);
            }
            else
            {
                transactions = await _accountService.GetAllTransactionsAsync(fromDate, toDate);
            }

            ViewBag.Accounts = accounts;
            ViewBag.SelectedAccountId = accountId;
            ViewBag.FromDate = fromDate?.ToString("yyyy-MM-dd");
            ViewBag.ToDate = toDate?.ToString("yyyy-MM-dd");

            return View(transactions);
        }

        // POST: Manual Deposit (AJAX)
        [HttpPost]
        [Permission("Accounts", "Edit")]
        public async Task<IActionResult> Deposit([FromBody] DepositWithdrawModel model)
        {
            try
            {
                if (model.AccountId <= 0 || model.Amount <= 0)
                {
                    return Json(new { success = false, message = IsBangla() ? "সঠিক তথ্য দিন" : "Invalid data" });
                }

                await _accountService.UpdateAccountBalanceAsync(
                    accountId: model.AccountId,
                    amount: model.Amount,
                    transactionType: "Income",
                    description: model.Description ?? (IsBangla() ? "ম্যানুয়াল জমা" : "Manual Deposit"),
                    createdBy: User.Identity?.Name ?? "Admin"
                );

                return Json(new { success = true, message = IsBangla() ? "টাকা জমা সফল হয়েছে!" : "Deposit successful!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while depositing money");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Manual Withdraw (AJAX)
        [HttpPost]
        [Permission("Accounts", "Edit")]
        public async Task<IActionResult> Withdraw([FromBody] DepositWithdrawModel model)
        {
            try
            {
                if (model.AccountId <= 0 || model.Amount <= 0)
                {
                    return Json(new { success = false, message = IsBangla() ? "সঠিক তথ্য দিন" : "Invalid data" });
                }

                await _accountService.UpdateAccountBalanceAsync(
                    accountId: model.AccountId,
                    amount: model.Amount,
                    transactionType: "Expense",
                    description: model.Description ?? (IsBangla() ? "ম্যানুয়াল উত্তোলন" : "Manual Withdrawal"),
                    createdBy: User.Identity?.Name ?? "Admin"
                );

                return Json(new { success = true, message = IsBangla() ? "টাকা উত্তোলন সফল হয়েছে!" : "Withdrawal successful!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while withdrawing money");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // POST: Transfer Between Accounts (AJAX)
        [HttpPost]
        [Permission("Accounts", "Edit")]
        public async Task<IActionResult> Transfer([FromBody] TransferModel model)
        {
            try
            {
                if (model.FromAccountId <= 0 || model.ToAccountId <= 0 || model.Amount <= 0)
                {
                    return Json(new { success = false, message = IsBangla() ? "সঠিক তথ্য দিন" : "Invalid data" });
                }

                if (model.FromAccountId == model.ToAccountId)
                {
                    return Json(new { success = false, message = IsBangla() ? "একই একাউন্টে ট্রান্সফার করা যাবে না" : "Cannot transfer to same account" });
                }

                var fromAccount = await _accountService.GetAccountByIdAsync(model.FromAccountId);
                var toAccount = await _accountService.GetAccountByIdAsync(model.ToAccountId);

                if (fromAccount == null || toAccount == null)
                {
                    return Json(new { success = false, message = IsBangla() ? "একাউন্ট পাওয়া যায়নি" : "Account not found" });
                }

                // Check sufficient balance
                if (!await _accountService.CheckSufficientBalanceAsync(model.FromAccountId, model.Amount))
                {
                    return Json(new { success = false, message = IsBangla() ? "অপর্যাপ্ত ব্যালেন্স" : "Insufficient balance" });
                }

                var description = model.Description ?? (IsBangla() ? 
                    $"{fromAccount.AccountName} থেকে {toAccount.AccountName} এ স্থানান্তর" : 
                    $"Transfer from {fromAccount.AccountName} to {toAccount.AccountName}");

                // Deduct from source account
                await _accountService.UpdateAccountBalanceAsync(
                    accountId: model.FromAccountId,
                    amount: model.Amount,
                    transactionType: "transfer_out",
                    description: description,
                    referenceType: "Transfer",
                    referenceId: model.ToAccountId,
                    createdBy: User.Identity?.Name ?? "Admin"
                );

                // Add to destination account
                await _accountService.UpdateAccountBalanceAsync(
                    accountId: model.ToAccountId,
                    amount: model.Amount,
                    transactionType: "transfer_in",
                    description: description,
                    referenceType: "Transfer",
                    referenceId: model.FromAccountId,
                    createdBy: User.Identity?.Name ?? "Admin"
                );

                return Json(new { success = true, message = IsBangla() ? "ট্রান্সফার সফল হয়েছে!" : "Transfer successful!" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while transferring between accounts");
                return Json(new { success = false, message = ex.Message });
            }
        }

        // GET: Account Summary (AJAX)
        [HttpGet]
        [Permission("Accounts", "View")]
        public async Task<IActionResult> GetAccountSummary(int id)
        {
            try
            {
                _logger.LogInformation($"Getting account summary for account ID: {id}");
                
                var summary = await _accountService.GetAccountSummaryAsync(id);
                
                _logger.LogInformation($"Account summary retrieved successfully for ID: {id}");
                
                return Json(new { success = true, data = summary });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, $"Account not found: {id}");
                return Json(new { success = false, message = IsBangla() ? "একাউন্ট পাওয়া যায়নি" : "Account not found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting account summary for ID: {id}");
                return Json(new { success = false, message = IsBangla() ? "একটি ত্রুটি ঘটেছে: " + ex.Message : "An error occurred: " + ex.Message });
            }
        }
    }

    // Helper Models
    public class DepositWithdrawModel
    {
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }

    public class TransferModel
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public string? Description { get; set; }
    }
}

