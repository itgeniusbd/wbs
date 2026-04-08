using System.Net;
using System.Text;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;
using WBS.Web.Models;

namespace WBS.Web.Services
{
    public class GreenwebSmsService : ISmsService
    {
        private readonly string _apiToken;
        private readonly string _apiUrl = "https://api.bdbulksms.net/api.php";
        private readonly ILogger<GreenwebSmsService> _logger;
        private readonly bool _isEnabled;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IHttpClientFactory _httpClientFactory;

        public GreenwebSmsService(
            IConfiguration configuration, 
            ILogger<GreenwebSmsService> logger,
            IServiceScopeFactory scopeFactory,
            IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _httpClientFactory = httpClientFactory;
            _apiToken = configuration["GreenwebSms:ApiToken"] ?? "";
            _isEnabled = configuration.GetValue<bool>("GreenwebSms:Enabled", false);

            if (string.IsNullOrEmpty(_apiToken) && _isEnabled)
            {
                _logger.LogWarning("Greenweb SMS API Token is not configured. SMS sending will be disabled.");
                _isEnabled = false;
            }
        }

        public async Task<bool> SendSmsAsync(string phoneNumber, string message)
        {
            if (!_isEnabled)
            {
                _logger.LogInformation("SMS is disabled. Would have sent to {Phone}: {Message}", phoneNumber, message);
                await LogSmsAsync(phoneNumber, message, SmsStatus.Failed, 0, 0, "SMS service is disabled", null, null, null);
                return false;
            }

            // Clean phone number
            phoneNumber = CleanPhoneNumber(phoneNumber);

            // Validate phone number
            if (!IsValidBangladeshiNumber(phoneNumber))
            {
                _logger.LogWarning("Invalid Bangladeshi phone number: {Phone}", phoneNumber);
                await LogSmsAsync(phoneNumber, message, SmsStatus.InvalidNumber, 0, 0, "Invalid phone number", null, null, null);
                return false;
            }

            // Check SMS balance
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var smsBalance = await context.SmsBalances.OrderByDescending(s => s.Id).FirstOrDefaultAsync();
            if (smsBalance == null || smsBalance.AvailableBalance <= 0)
            {
                _logger.LogWarning("Insufficient SMS balance. Available: {Balance}", smsBalance?.AvailableBalance ?? 0);
                await LogSmsAsync(phoneNumber, message, SmsStatus.InsufficientBalance, 
                    smsBalance?.AvailableBalance ?? 0, 
                    smsBalance?.AvailableBalance ?? 0, 
                    "Insufficient SMS balance", null, null, null);
                return false;
            }

            int balanceBefore = smsBalance.AvailableBalance;

            try
            {
                // Escape message for URL
                string escapedMessage = Uri.EscapeDataString(message);

                // Build API URL
                string url = $"{_apiUrl}?token={_apiToken}&to={phoneNumber}&message={escapedMessage}";

                _logger.LogInformation("Sending SMS to {Phone}", phoneNumber);

                // Send request using HttpClient
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                
                var response = await httpClient.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation("SMS API Response: {Response}", result);

                // Check if successful - Greenweb returns "Ok: SMS Sent Successfully" or "SUCCESS"
                if (result.Contains("Ok:", StringComparison.OrdinalIgnoreCase) || 
                    result.Contains("SUCCESS", StringComparison.OrdinalIgnoreCase) ||
                    result.Contains("Sent Successfully", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogInformation("SMS sent successfully to {Phone}", phoneNumber);

                    // Deduct balance
                    smsBalance.AvailableBalance--;
                    smsBalance.LastUpdated = DateTime.UtcNow;
                    await context.SaveChangesAsync();

                    int balanceAfter = smsBalance.AvailableBalance;

                    // Log success
                    await LogSmsAsync(phoneNumber, message, SmsStatus.Success, 
                        balanceBefore, balanceAfter, result, null, null, null);

                    return true;
                }
                else
                {
                    _logger.LogWarning("SMS sending failed. Response: {Response}", result);
                    await LogSmsAsync(phoneNumber, message, SmsStatus.Failed, 
                        balanceBefore, balanceBefore, result, null, null, null);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending SMS to {Phone}", phoneNumber);
                await LogSmsAsync(phoneNumber, message, SmsStatus.Failed, 
                    balanceBefore, balanceBefore, ex.Message, null, null, null);
                return false;
            }
        }

        public async Task<bool> SendBulkSmsAsync(List<string> phoneNumbers, string message)
        {
            if (!_isEnabled)
            {
                _logger.LogInformation("SMS is disabled. Would have sent bulk SMS to {Count} numbers", phoneNumbers.Count);
                return false;
            }

            // Clean and validate all numbers
            var validNumbers = phoneNumbers
                .Select(CleanPhoneNumber)
                .Where(IsValidBangladeshiNumber)
                .Distinct()
                .ToList();

            if (!validNumbers.Any())
            {
                _logger.LogWarning("No valid phone numbers found for bulk SMS");
                return false;
            }

            // Check SMS balance
            using var scope = _scopeFactory.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var smsBalance = await context.SmsBalances.OrderByDescending(s => s.Id).FirstOrDefaultAsync();
            if (smsBalance == null || smsBalance.AvailableBalance < validNumbers.Count)
            {
                _logger.LogWarning("Insufficient SMS balance for bulk send. Need: {Need}, Available: {Available}", 
                    validNumbers.Count, smsBalance?.AvailableBalance ?? 0);
                return false;
            }

            int balanceBefore = smsBalance.AvailableBalance;

            try
            {
                // Join numbers with comma
                string numbersString = string.Join(",", validNumbers);

                // Escape message for URL
                string escapedMessage = Uri.EscapeDataString(message);

                // Build API URL
                string url = $"{_apiUrl}?token={_apiToken}&to={numbersString}&message={escapedMessage}";

                _logger.LogInformation("Sending bulk SMS to {Count} numbers", validNumbers.Count);

                // Send request using HttpClient
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                
                var response = await httpClient.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation("Bulk SMS API Response: {Response}", result);

                // Check if successful - Greenweb returns "Ok: SMS Sent Successfully" or "SUCCESS"
                if (result.Contains("Ok:", StringComparison.OrdinalIgnoreCase) || 
                    result.Contains("SUCCESS", StringComparison.OrdinalIgnoreCase) ||
                    result.Contains("Sent Successfully", StringComparison.OrdinalIgnoreCase))
                {
                    // Deduct balance for all sent messages
                    smsBalance.AvailableBalance -= validNumbers.Count;
                    smsBalance.LastUpdated = DateTime.UtcNow;
                    await context.SaveChangesAsync();

                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending bulk SMS");
                return false;
            }
        }

        public async Task<bool> SendDonationReceiptAsync(string phoneNumber, string donorName, decimal amount, string transactionId)
        {
            // Clean phone number
            phoneNumber = CleanPhoneNumber(phoneNumber);

            // Validate phone number
            if (!IsValidBangladeshiNumber(phoneNumber))
            {
                _logger.LogWarning("Invalid phone number for donation receipt: {Phone}", phoneNumber);
                return false;
            }

            if (!_isEnabled)
            {
                _logger.LogInformation("SMS is disabled. Would have sent donation receipt to {Phone}", phoneNumber);
                await LogSmsAsync(phoneNumber, "", SmsStatus.Failed, 0, 0, 
                    "SMS service is disabled", donorName, amount, transactionId);
                return false;
            }

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Find donation by transaction ID to get donation type
                var donation = await context.Donations
                    .Include(d => d.DonationType)
                    .FirstOrDefaultAsync(d => d.TransactionId == transactionId);

                string donationTypeName = donation?.DonationType?.Name ?? "General Donation";
                int? donationTypeId = donation?.DonationTypeId;

                // Get SMS template for this donation type or default
                var template = await context.NotificationTemplates
                    .Where(t => t.TemplateType == "SMS" && t.IsActive)
                    .Where(t => t.DonationTypeId == donationTypeId || (t.DonationTypeId == null && t.IsDefault))
                    .OrderByDescending(t => t.DonationTypeId.HasValue)
                    .ThenByDescending(t => t.IsDefault)
                    .FirstOrDefaultAsync();

                string message;

                if (template != null && !string.IsNullOrEmpty(template.SmsContent))
                {
                    // Use template
                    message = template.SmsContent
                        .Replace("{DonorName}", donorName)
                        .Replace("{Amount}", amount.ToString("N2"))
                        .Replace("{DonationType}", donationTypeName)
                        .Replace("{TransactionId}", transactionId);
                }
                else
                {
                    // Fallback to default message
                    message = $"Dear {donorName}, Thank you for your generous donation of BDT {amount:N2} to {donationTypeName}. " +
                           $"Transaction ID: {transactionId}. May Allah accept your charity. - WBS";
                }

                // Check SMS balance
                var smsBalance = await context.SmsBalances.OrderByDescending(s => s.Id).FirstOrDefaultAsync();
                if (smsBalance == null || smsBalance.AvailableBalance <= 0)
                {
                    _logger.LogWarning("Insufficient SMS balance for donation receipt. Available: {Balance}", 
                        smsBalance?.AvailableBalance ?? 0);
                    await LogSmsAsync(phoneNumber, message, SmsStatus.InsufficientBalance, 
                        smsBalance?.AvailableBalance ?? 0, 
                        smsBalance?.AvailableBalance ?? 0, 
                        "Insufficient SMS balance", donorName, amount, transactionId);
                    return false;
                }

                int balanceBefore = smsBalance.AvailableBalance;

                // Escape message for URL
                string escapedMessage = Uri.EscapeDataString(message);

                // Build API URL
                string url = $"{_apiUrl}?token={_apiToken}&to={phoneNumber}&message={escapedMessage}";

                _logger.LogInformation("Sending donation receipt SMS to {Phone}", phoneNumber);

                // Send request using HttpClient
                var httpClient = _httpClientFactory.CreateClient();
                httpClient.Timeout = TimeSpan.FromSeconds(30);
                
                var response = await httpClient.GetAsync(url);
                var result = await response.Content.ReadAsStringAsync();
                
                _logger.LogInformation("Donation receipt SMS API Response: {Response}", result);

                // Check if successful - Greenweb returns "Ok: SMS Sent Successfully" or "SUCCESS"
                if (result.Contains("Ok:", StringComparison.OrdinalIgnoreCase) || 
                    result.Contains("SUCCESS", StringComparison.OrdinalIgnoreCase) ||
                    result.Contains("Sent Successfully", StringComparison.OrdinalIgnoreCase))
                {
                    _logger.LogInformation("Donation receipt SMS sent successfully to {Phone}", phoneNumber);

                    // Deduct balance
                    smsBalance.AvailableBalance--;
                    smsBalance.LastUpdated = DateTime.UtcNow;
                    await context.SaveChangesAsync();

                    int balanceAfter = smsBalance.AvailableBalance;

                    // Log success
                    await LogSmsAsync(phoneNumber, message, SmsStatus.Success, 
                        balanceBefore, balanceAfter, result, donorName, amount, transactionId);

                    return true;
                }
                else
                {
                    _logger.LogWarning("Donation receipt SMS failed. Response: {Response}", result);
                    await LogSmsAsync(phoneNumber, message, SmsStatus.Failed, 
                        balanceBefore, balanceBefore, result, donorName, amount, transactionId);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending donation receipt SMS to {Phone}", phoneNumber);
                await LogSmsAsync(phoneNumber, "", SmsStatus.Failed, 
                    0, 0, ex.Message, donorName, amount, transactionId);
                return false;
            }
        }

        public async Task<int> GetSmsBalanceAsync()
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var smsBalance = await context.SmsBalances
                    .OrderByDescending(s => s.Id)
                    .FirstOrDefaultAsync();

                return smsBalance?.AvailableBalance ?? 0;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting SMS balance");
                return 0;
            }
        }

        #region Helper Methods

        private async Task LogSmsAsync(
            string phoneNumber, 
            string message, 
            SmsStatus status,
            int balanceBefore,
            int balanceAfter,
            string? errorMessage,
            string? donorName,
            decimal? amount,
            string? transactionId)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                var log = new SmsLog
                {
                    PhoneNumber = phoneNumber,
                    Message = message,
                    Status = status,
                    ErrorMessage = errorMessage,
                    SentAt = DateTime.UtcNow,
                    DonorName = donorName,
                    Amount = amount,
                    TransactionId = transactionId,
                    BalanceBefore = balanceBefore,
                    BalanceAfter = balanceAfter
                };

                context.SmsLogs.Add(log);
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging SMS");
            }
        }

        private string CleanPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return "";

            // Remove all non-digit characters
            string cleaned = new string(phoneNumber.Where(char.IsDigit).ToArray());

            // If starts with +880, remove it
            if (cleaned.StartsWith("880") && cleaned.Length == 13)
            {
                cleaned = "0" + cleaned.Substring(3);
            }

            // If starts with 88, remove it
            if (cleaned.StartsWith("88") && cleaned.Length == 12)
            {
                cleaned = "0" + cleaned.Substring(2);
            }

            return cleaned;
        }

        private bool IsValidBangladeshiNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
                return false;

            // Must be 11 digits starting with 01
            if (phoneNumber.Length != 11)
                return false;

            if (!phoneNumber.StartsWith("01"))
                return false;

            // Valid operator codes: 013, 014, 015, 016, 017, 018, 019
            string operatorCode = phoneNumber.Substring(0, 3);
            var validOperators = new[] { "013", "014", "015", "016", "017", "018", "019" };

            return validOperators.Contains(operatorCode);
        }

        #endregion
    }
}
