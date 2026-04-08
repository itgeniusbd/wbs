using System.Net;
using System.Net.Mail;
using Microsoft.EntityFrameworkCore;
using WBS.Web.Data;

namespace WBS.Web.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;
        private readonly bool _isEnabled;
        private readonly IServiceScopeFactory _scopeFactory;

        public EmailService(
            IConfiguration configuration, 
            ILogger<EmailService> logger,
            IServiceScopeFactory scopeFactory)
        {
            _configuration = configuration;
            _logger = logger;
            _scopeFactory = scopeFactory;
            _isEnabled = configuration.GetValue<bool>("EmailSettings:Enabled", false);
        }

        public async Task<bool> SendEmailAsync(string toEmail, string subject, string htmlBody)
        {
            if (!_isEnabled)
            {
                _logger.LogInformation("Email is disabled. Would have sent to {Email}: {Subject}", toEmail, subject);
                return false;
            }

            try
            {
                var smtpServer = _configuration["EmailSettings:SmtpServer"];
                var smtpPort = _configuration.GetValue<int>("EmailSettings:SmtpPort", 587);
                var fromEmail = _configuration["EmailSettings:FromEmail"];
                var fromName = _configuration["EmailSettings:FromName"];
                var username = _configuration["EmailSettings:Username"];
                var password = _configuration["EmailSettings:Password"];
                var enableSsl = _configuration.GetValue<bool>("EmailSettings:EnableSsl", true);

                if (string.IsNullOrEmpty(smtpServer) || string.IsNullOrEmpty(fromEmail))
                {
                    _logger.LogWarning("Email settings are not configured properly");
                    return false;
                }

                using (var client = new SmtpClient(smtpServer, smtpPort))
                {
                    client.EnableSsl = enableSsl;
                    client.Credentials = new NetworkCredential(username ?? fromEmail, password);

                    var mailMessage = new MailMessage
                    {
                        From = new MailAddress(fromEmail, fromName ?? "WBS"),
                        Subject = subject,
                        Body = htmlBody,
                        IsBodyHtml = true
                    };

                    mailMessage.To.Add(toEmail);

                    await client.SendMailAsync(mailMessage);
                    _logger.LogInformation("Email sent successfully to {Email}", toEmail);
                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email to {Email}", toEmail);
                return false;
            }
        }

        public async Task<bool> SendDonationReceiptAsync(string toEmail, string donorName, decimal amount, string transactionId, int donationId)
        {
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

                // Get donation details
                var donation = await context.Donations
                    .Include(d => d.DonationType)
                    .FirstOrDefaultAsync(d => d.Id == donationId);

                if (donation == null)
                {
                    _logger.LogWarning("Donation {DonationId} not found", donationId);
                    return false;
                }

                string donationTypeName = donation.DonationType?.Name ?? "General Donation";
                int? donationTypeId = donation.DonationTypeId;

                // Get email template for this donation type or default
                var template = await context.NotificationTemplates
                    .Where(t => t.TemplateType == "Email" && t.IsActive)
                    .Where(t => t.DonationTypeId == donationTypeId || (t.DonationTypeId == null && t.IsDefault))
                    .OrderByDescending(t => t.DonationTypeId.HasValue)
                    .ThenByDescending(t => t.IsDefault)
                    .FirstOrDefaultAsync();

                string subject;
                string htmlBody;

                if (template != null)
                {
                    // Use template
                    subject = template.EmailSubject!
                        .Replace("{TransactionId}", transactionId)
                        .Replace("{DonorName}", donorName)
                        .Replace("{Amount}", amount.ToString("N2"))
                        .Replace("{DonationType}", donationTypeName)
                        .Replace("{Date}", DateTime.Now.ToString("dd MMM yyyy"));

                    htmlBody = template.EmailContent!
                        .Replace("{DonorName}", donorName)
                        .Replace("{Amount}", amount.ToString("N2"))
                        .Replace("{DonationType}", donationTypeName)
                        .Replace("{TransactionId}", transactionId)
                        .Replace("{Date}", DateTime.Now.ToString("dd MMMM yyyy, hh:mm tt"));
                }
                else
                {
                    // Fallback to default template
                    subject = $"Thank You for Your Donation - Receipt #{donationId}";

                    htmlBody = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
        .header {{ background: linear-gradient(135deg, #2c5f2d 0%, #1a3a1b 100%); color: white; padding: 30px; text-align: center; border-radius: 10px 10px 0 0; }}
        .logo {{ max-width: 120px; margin-bottom: 15px; }}
        .content {{ background: #f8f9fa; padding: 30px; }}
        .receipt-box {{ background: white; padding: 20px; border-radius: 8px; margin: 20px 0; border-left: 4px solid #28a745; }}
        .amount {{ font-size: 32px; font-weight: bold; color: #28a745; }}
        .footer {{ text-align: center; padding: 20px; color: #666; font-size: 14px; }}
        .button {{ display: inline-block; padding: 12px 30px; background: #2c5f2d; color: white; text-decoration: none; border-radius: 5px; margin-top: 20px; }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <img src='https://yourwebsite.com/images/logo.png' alt='WBS Logo' class='logo' />
            <h1>WBS</h1>
            <p>Thank You for Your Generosity!</p>
        </div>
        
        <div class='content'>
            <h2>Dear {donorName},</h2>
            <p>Assalamu Alaikum! We are deeply grateful for your generous donation. Your contribution will help us make a positive impact in the community.</p>
            
            <div class='receipt-box'>
                <h3>Donation Receipt</h3>
                <table style='width: 100%;'>
                    <tr>
                        <td><strong>Receipt Number:</strong></td>
                        <td>#{donationId}</td>
                    </tr>
                    <tr>
                        <td><strong>Transaction ID:</strong></td>
                        <td>{transactionId}</td>
                    </tr>
                    <tr>
                        <td><strong>Donation Type:</strong></td>
                        <td>{donationTypeName}</td>
                    </tr>
                    <tr>
                        <td><strong>Amount:</strong></td>
                        <td><span class='amount'>?{amount:N2}</span></td>
                    </tr>
                    <tr>
                        <td><strong>Date:</strong></td>
                        <td>{DateTime.Now:dd MMMM yyyy, hh:mm tt}</td>
                    </tr>
                </table>
            </div>
            
            <p><strong>May Allah accept your charity and bless you abundantly.</strong></p>
            
            <p>This is an automated receipt for your donation. You can view and print your invoice anytime by clicking the button below:</p>
            
            <center>
                <a href='https://yourwebsite.com/admin/donations/invoice/{donationId}' class='button'>View Invoice</a>
            </center>
            
            <p style='margin-top: 30px;'>If you have any questions, please don't hesitate to contact us.</p>
        </div>
        
        <div class='footer'>
            <p><strong>WBS</strong></p>
            <p>Working for Humanity</p>
            <p>Email: info@wbs.org | Phone: +880 1XXX-XXXXXX</p>
            <p>&copy; {DateTime.Now.Year} WBS. All rights reserved.</p>
        </div>
    </div>
</body>
</html>";
                }

                return await SendEmailAsync(toEmail, subject, htmlBody);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending donation receipt email");
                return false;
            }
        }
    }
}
