namespace WBS.Web.Services
{
    public interface IEmailService
    {
        Task<bool> SendEmailAsync(string toEmail, string subject, string htmlBody);
        Task<bool> SendDonationReceiptAsync(string toEmail, string donorName, decimal amount, string transactionId, int donationId);
    }
}
