using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;

namespace WBS.Web.Controllers
{
    public class DiagnosticsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<DiagnosticsController> _logger;
        private readonly IConfiguration _configuration;

        public DiagnosticsController(
            ApplicationDbContext context,
            ILogger<DiagnosticsController> logger,
            IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var diagnostics = new Dictionary<string, object>();

            try
            {
                // 1. Check database connection
                try
                {
                    await _context.Database.CanConnectAsync();
                    diagnostics["DatabaseConnection"] = "? Connected";
                    diagnostics["ConnectionString"] = MaskConnectionString(_configuration.GetConnectionString("DefaultConnection"));
                }
                catch (Exception dbEx)
                {
                    diagnostics["DatabaseConnection"] = $"? Failed: {dbEx.Message}";
                    diagnostics["DatabaseError"] = dbEx.ToString();
                }

                // 2. Check DonationTypes table
                try
                {
                    var donationTypesCount = await _context.DonationTypes.CountAsync();
                    var activeDonationTypes = await _context.DonationTypes.Where(dt => dt.IsActive).CountAsync();
                    diagnostics["DonationTypesTotal"] = donationTypesCount;
                    diagnostics["DonationTypesActive"] = activeDonationTypes;

                    if (activeDonationTypes > 0)
                    {
                        var types = await _context.DonationTypes
                            .Where(dt => dt.IsActive)
                            .Select(dt => new { dt.Id, dt.Name, dt.DisplayOrder })
                            .ToListAsync();
                        diagnostics["DonationTypesList"] = types;
                    }
                }
                catch (Exception dtEx)
                {
                    diagnostics["DonationTypesError"] = $"? {dtEx.Message}";
                }

                // 3. Check Appeals
                try
                {
                    var appealsCount = await _context.Appeals.CountAsync();
                    diagnostics["AppealsTotal"] = appealsCount;
                }
                catch (Exception appEx)
                {
                    diagnostics["AppealsError"] = $"? {appEx.Message}";
                }

                // 4. Check Accounts
                try
                {
                    var accountsCount = await _context.Accounts.CountAsync();
                    var defaultAccount = await _context.Accounts
                        .Where(a => a.IsActive && a.Default_Status)
                        .FirstOrDefaultAsync();
                    diagnostics["AccountsTotal"] = accountsCount;
                    diagnostics["DefaultAccount"] = defaultAccount != null ? $"{defaultAccount.Id} - {defaultAccount.AccountName}" : "? No default account";
                }
                catch (Exception accEx)
                {
                    diagnostics["AccountsError"] = $"? {accEx.Message}";
                }

                // 5. Check Donations
                try
                {
                    var donationsCount = await _context.Donations.CountAsync();
                    var recentDonation = await _context.Donations
                        .OrderByDescending(d => d.CreatedAt)
                        .Select(d => new { d.Id, d.DonorName, d.Amount, d.CreatedAt })
                        .FirstOrDefaultAsync();
                    diagnostics["DonationsTotal"] = donationsCount;
                    diagnostics["LatestDonation"] = recentDonation;
                }
                catch (Exception donEx)
                {
                    diagnostics["DonationsError"] = $"? {donEx.Message}";
                }

                // 6. Check SSLCommerz Configuration
                try
                {
                    var sslConfig = new
                    {
                        StoreId = _configuration["SSLCommerz:StoreId"],
                        IsLive = _configuration["SSLCommerz:IsLive"],
                        SessionUrl = _configuration["SSLCommerz:SessionUrl"]
                    };
                    diagnostics["SSLCommerzConfig"] = sslConfig;
                }
                catch (Exception sslEx)
                {
                    diagnostics["SSLCommerzError"] = $"? {sslEx.Message}";
                }

                // 7. Environment Info
                diagnostics["Environment"] = new
                {
                    AspNetCoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                    MachineName = Environment.MachineName,
                    OSVersion = Environment.OSVersion.ToString(),
                    ProcessorCount = Environment.ProcessorCount,
                    WorkingSet = $"{Environment.WorkingSet / 1024 / 1024} MB"
                };

            }
            catch (Exception ex)
            {
                diagnostics["GeneralError"] = ex.ToString();
            }

            return View(diagnostics);
        }

        private string MaskConnectionString(string? connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return "Not configured";

            // Mask password
            var parts = connectionString.Split(';');
            for (int i = 0; i < parts.Length; i++)
            {
                if (parts[i].Contains("Password", StringComparison.OrdinalIgnoreCase) ||
                    parts[i].Contains("Pwd", StringComparison.OrdinalIgnoreCase))
                {
                    parts[i] = "Password=****";
                }
            }
            return string.Join(";", parts);
        }
    }
}
