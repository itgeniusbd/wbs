namespace WBS.Web.Services
{
    public interface ISmsService
    {
        Task<bool> SendSmsAsync(string phoneNumber, string message);
        Task<bool> SendBulkSmsAsync(List<string> phoneNumbers, string message);
        Task<bool> SendDonationReceiptAsync(string phoneNumber, string donorName, decimal amount, string transactionId);
        Task<int> GetSmsBalanceAsync();
    }
}
