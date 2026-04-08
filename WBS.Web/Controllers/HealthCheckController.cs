using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Services;

namespace WBS.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthCheckController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<HealthCheckController> _logger;
        private readonly IConfiguration _configuration;
        private readonly ISmsService _smsService;
        private readonly IEmailService _emailService;

        public HealthCheckController(
            ApplicationDbContext context,
            ILogger<HealthCheckController> logger,
            IConfiguration configuration,
            ISmsService smsService,
            IEmailService emailService)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            _smsService = smsService;
            _emailService = emailService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var health = new
            {
                Status = "Healthy",
                Timestamp = DateTime.UtcNow,
                Environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
                Checks = new Dictionary<string, object>()
            };

            try
            {
                // Database check
                try
                {
                    var canConnect = await _context.Database.CanConnectAsync();
                    health.Checks.Add("Database", new
                    {
                        Status = canConnect ? "Connected" : "Failed",
                        ConnectionString = MaskConnectionString(_configuration.GetConnectionString("DefaultConnection") ?? "Not configured")
                    });
                }
                catch (Exception ex)
                {
                    health.Checks.Add("Database", new
                    {
                        Status = "Error",
                        Error = ex.Message
                    });
                }

                // Email configuration check
                var emailEnabled = _configuration.GetValue<bool>("EmailSettings:Enabled");
                var emailServer = _configuration["EmailSettings:SmtpServer"];
                health.Checks.Add("Email", new
                {
                    Status = emailEnabled ? "Enabled" : "Disabled",
                    SmtpServer = emailServer ?? "Not configured"
                });

                // SMS configuration check
                var smsEnabled = _configuration.GetValue<bool>("GreenwebSms:Enabled");
                var hasApiToken = !string.IsNullOrEmpty(_configuration["GreenwebSms:ApiToken"]);
                health.Checks.Add("SMS", new
                {
                    Status = smsEnabled ? "Enabled" : "Disabled",
                    ApiConfigured = hasApiToken
                });

                // Check donation types
                try
                {
                    var donationTypeCount = await _context.DonationTypes.CountAsync();
                    health.Checks.Add("DonationTypes", new
                    {
                        Status = "OK",
                        Count = donationTypeCount
                    });
                }
                catch (Exception ex)
                {
                    health.Checks.Add("DonationTypes", new
                    {
                        Status = "Error",
                        Error = ex.Message
                    });
                }

                return Ok(health);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Health check failed");
                return StatusCode(500, new
                {
                    Status = "Unhealthy",
                    Error = ex.Message,
                    Timestamp = DateTime.UtcNow
                });
            }
        }

        [HttpGet("test-donation-save")]
        public async Task<IActionResult> TestDonationSave()
        {
            try
            {
                // Create a test donation without saving
                var testDonation = new Models.Donation
                {
                    DonorName = "Test Donor",
                    Email = "test@example.com",
                    Phone = "01700000000",
                    Amount = 100,
                    DonationTypeId = 1,
                    PaymentMethod = "Test",
                    TransactionId = "TEST-" + DateTime.UtcNow.Ticks
                };

                // Try to add but don't save
                _context.Donations.Add(testDonation);
                
                return Ok(new
                {
                    Status = "Test successful - donation can be created",
                    Note = "No data was saved to database"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Test donation creation failed");
                return StatusCode(500, new
                {
                    Status = "Test failed",
                    Error = ex.Message,
                    InnerError = ex.InnerException?.Message
                });
            }
        }

        private string MaskConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                return "Not configured";

            // Mask password
            var parts = connectionString.Split(';');
            var masked = parts.Select(part =>
            {
                if (part.Trim().StartsWith("Password=", StringComparison.OrdinalIgnoreCase) ||
                    part.Trim().StartsWith("Pwd=", StringComparison.OrdinalIgnoreCase))
                {
                    return "Password=****";
                }
                return part;
            });

            return string.Join(";", masked);
        }
    }
}
