using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class SmsLog
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        [Required]
        public SmsStatus Status { get; set; }

        [StringLength(500)]
        public string? ErrorMessage { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        [StringLength(100)]
        public string? DonorName { get; set; }

        public int? DonationId { get; set; }
        public Donation? Donation { get; set; }

        public decimal? Amount { get; set; }

        [StringLength(100)]
        public string? TransactionId { get; set; }

        public int BalanceBefore { get; set; }
        public int BalanceAfter { get; set; }
    }

    public enum SmsStatus
    {
        Success = 1,
        Failed = 2,
        InsufficientBalance = 3,
        InvalidNumber = 4
    }
}
