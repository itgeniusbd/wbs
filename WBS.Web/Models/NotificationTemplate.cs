using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class NotificationTemplate
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string TemplateType { get; set; } = string.Empty; // SMS, Email

        [StringLength(50)]
        public string? Category { get; set; } // DonationReceipt, Welcome, etc.

        public int? DonationTypeId { get; set; }
        public DonationType? DonationType { get; set; }

        [StringLength(200)]
        public string? EmailSubject { get; set; }

        public string? SmsContent { get; set; }

        public string? EmailContent { get; set; }

        [StringLength(500)]
        public string? AvailablePlaceholders { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsDefault { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [StringLength(100)]
        public string? CreatedBy { get; set; }
    }
}
