using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class Donation
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string DonorName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Phone]
        [StringLength(20)]
        public string Phone { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(10, 999999999, ErrorMessage = "Minimum donation amount is ৳10")]
        public decimal Amount { get; set; }

        public string Currency { get; set; } = "BDT";

        [Required(ErrorMessage = "Please select a donation type")]
        public int DonationTypeId { get; set; }
        public DonationType? DonationType { get; set; }

        public int? AppealId { get; set; }
        public Appeal? Appeal { get; set; }

        public int? SDGId { get; set; }
        public SDG? SDG { get; set; }

        public int? ProgramId { get; set; }
        public SDGProgram? Program { get; set; }

        [Required(ErrorMessage = "Please select a donor type")]
        [StringLength(50)]
        public string DonorType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a payment method")]
        [StringLength(50)]
        public string PaymentMethod { get; set; } = string.Empty;

        [StringLength(100)]
        public string? TransactionId { get; set; }

        public DonationStatus Status { get; set; } = DonationStatus.Pending;

        // SSLCommerz payment tracking fields
        [StringLength(50)]
        public string? PaymentStatus { get; set; }
        
        public DateTime? PaymentDate { get; set; }
        
        [StringLength(100)]
        public string? BankTransactionId { get; set; }
        
        [StringLength(50)]
        public string? CardType { get; set; }

        public bool IsRecurring { get; set; } = false;
        public string? RecurringFrequency { get; set; }

        public bool IsAnonymous { get; set; } = false;
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }

        // Account tracking (automatically set to default account for public donations)
        public int? AccountId { get; set; }
        public Account? Account { get; set; }
    }

    public enum DonationStatus
    {
        Pending = 0,
        Completed = 1,
        Failed = 2,
        Refunded = 3,
        Cancelled = 4
    }
}
