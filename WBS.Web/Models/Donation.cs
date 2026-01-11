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

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [Required]
        public decimal Amount { get; set; }

        public string Currency { get; set; } = "BDT";

        public int DonationTypeId { get; set; }
        public DonationType? DonationType { get; set; }

        public int? AppealId { get; set; }
        public Appeal? Appeal { get; set; }

        public int? SDGId { get; set; }
        public SDG? SDG { get; set; }

        public int? ProgramId { get; set; }
        public SDGProgram? Program { get; set; }

        [StringLength(50)]
        public string PaymentMethod { get; set; } = string.Empty;

        [StringLength(100)]
        public string? TransactionId { get; set; }

        public DonationStatus Status { get; set; } = DonationStatus.Pending;

        public bool IsRecurring { get; set; } = false;
        public string? RecurringFrequency { get; set; }

        public bool IsAnonymous { get; set; } = false;
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? PaidAt { get; set; }
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
