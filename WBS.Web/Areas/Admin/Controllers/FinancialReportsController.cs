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
    public class FinancialReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<FinancialReportsController> _logger;

        public FinancialReportsController(
            ApplicationDbContext context,
            ILogger<FinancialReportsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: Admin/FinancialReports
        [Permission("Financial Reports", "View")]
        public async Task<IActionResult> Index(DateTime? fromDate, DateTime? toDate, int? accountId)
        {
            try
            {
                // Set default date range if not provided
                if (!fromDate.HasValue)
                    fromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); // First day of current month

                if (!toDate.HasValue)
                    toDate = DateTime.Now;

                // Income Sources
                var donations = await _context.Donations
                    .Include(d => d.DonationType)
                    .Include(d => d.Account)
                    .Where(d => d.Status == DonationStatus.Completed && 
                               d.PaidAt >= fromDate && d.PaidAt <= toDate &&
                               (!accountId.HasValue || d.AccountId == accountId))
                    .ToListAsync();

                // Online donations - PaymentMethod = "Online" (from SSL Commerz)
                var onlineDonations = donations
                    .Where(d => d.PaymentMethod?.ToLower() == "online")
                    .ToList();

                // Manual donations - Any other payment method (Cash, Bank Transfer, Cheque, etc.)
                var manualDonations = donations
                    .Where(d => d.PaymentMethod?.ToLower() != "online")
                    .ToList();

                var otherIncomes = await _context.OtherIncomes
                    .Where(i => i.IsActive && 
                               i.IncomeDate >= fromDate && i.IncomeDate <= toDate &&
                               (!accountId.HasValue || i.AccountId == accountId))
                    .ToListAsync();

                var eventRegistrations = await _context.EventRegistrations
                    .Where(r => r.Status == "Confirmed" && 
                               r.ConfirmedAt >= fromDate && r.ConfirmedAt <= toDate &&
                               r.AmountPaid > 0)
                    .ToListAsync();

                // Get deposits (account deposits/transfers IN)
                var deposits = await _context.AccountTransactions
                    .Include(t => t.Account)
                    .Where(t => t.TransactionType == "Income" && 
                               t.TransactionDate >= fromDate && t.TransactionDate <= toDate &&
                               (!accountId.HasValue || t.AccountId == accountId) &&
                               (t.ReferenceType == "Deposit" || t.Description.Contains("Deposit") || t.Description.Contains("জমা")))
                    .ToListAsync();

                // Expense Sources
                var generalExpenses = await _context.GeneralExpenses
                    .Where(e => e.IsActive && 
                               e.ExpenseDate >= fromDate && e.ExpenseDate <= toDate &&
                               (!accountId.HasValue || e.AccountId == accountId))
                    .ToListAsync();

                var programExpenses = await _context.ProgramExpenses
                    .Include(e => e.SDG)
                    .Include(e => e.Program)
                    .Where(e => e.IsActive && 
                               e.ExpenseDate >= fromDate && e.ExpenseDate <= toDate &&
                               (!accountId.HasValue || e.AccountId == accountId))
                    .ToListAsync();

                // Account Transactions (for withdrawals)
                var withdrawals = await _context.AccountTransactions
                    .Where(t => t.TransactionType == "Expense" && 
                               t.TransactionDate >= fromDate && t.TransactionDate <= toDate &&
                               (!accountId.HasValue || t.AccountId == accountId) &&
                               (t.ReferenceType == "Withdrawal" || t.Description.Contains("Withdrawal")))
                    .ToListAsync();

                // Calculate totals
                var totalOnlineDonations = onlineDonations.Sum(d => d.Amount);
                var totalManualDonations = manualDonations.Sum(d => d.Amount);
                var totalOtherIncomes = otherIncomes.Sum(i => i.Amount);
                var totalEventFees = eventRegistrations.Sum(r => r.AmountPaid);
                var totalDeposits = deposits.Sum(d => d.Amount);
                var totalIncome = totalOnlineDonations + totalManualDonations + totalOtherIncomes + totalEventFees + totalDeposits;

                var totalGeneralExpenses = generalExpenses.Sum(e => e.Amount);
                var totalProgramExpenses = programExpenses.Sum(e => e.Amount);
                var totalWithdrawals = withdrawals.Sum(w => w.Amount);
                var totalExpenses = totalGeneralExpenses + totalProgramExpenses + totalWithdrawals;

                var netBalance = totalIncome - totalExpenses;

                // Pass data to view
                ViewBag.FromDate = fromDate;
                ViewBag.ToDate = toDate;
                ViewBag.SelectedAccountId = accountId;

                ViewBag.TotalOnlineDonations = totalOnlineDonations;
                ViewBag.TotalManualDonations = totalManualDonations;
                ViewBag.TotalOtherIncomes = totalOtherIncomes;
                ViewBag.TotalEventFees = totalEventFees;
                ViewBag.TotalDeposits = totalDeposits;
                ViewBag.TotalIncome = totalIncome;

                ViewBag.TotalGeneralExpenses = totalGeneralExpenses;
                ViewBag.TotalProgramExpenses = totalProgramExpenses;
                ViewBag.TotalWithdrawals = totalWithdrawals;
                ViewBag.TotalExpenses = totalExpenses;

                ViewBag.NetBalance = netBalance;

                ViewBag.OnlineDonations = onlineDonations;
                ViewBag.ManualDonations = manualDonations;
                ViewBag.OtherIncomes = otherIncomes;
                ViewBag.EventRegistrations = eventRegistrations;
                ViewBag.Deposits = deposits;
                ViewBag.GeneralExpenses = generalExpenses;
                ViewBag.ProgramExpenses = programExpenses;
                ViewBag.Withdrawals = withdrawals;

                // Load accounts for filter
                ViewBag.Accounts = await _context.Accounts
                    .Where(a => a.IsActive)
                    .OrderBy(a => a.AccountName)
                    .ToListAsync();

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading financial report");
                TempData["Error"] = "Error loading financial report.";
                return View();
            }
        }

        // Export to Excel (optional)
        public async Task<IActionResult> ExportToExcel(DateTime? fromDate, DateTime? toDate, int? accountId)
        {
            // Implementation for Excel export can be added later
            TempData["Info"] = "Excel export feature coming soon!";
            return RedirectToAction(nameof(Index), new { fromDate, toDate, accountId });
        }

        // Print Report
        public async Task<IActionResult> Print(DateTime? fromDate, DateTime? toDate, int? accountId)
        {
            // Same logic as Index but return a print-friendly view
            return await Index(fromDate, toDate, accountId);
        }
    }
}
