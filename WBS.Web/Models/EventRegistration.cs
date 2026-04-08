using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WBS.Web.Models
{
    public class EventRegistration
    {
        public int Id { get; set; }

        public int EventId { get; set; }
        public Event? Event { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

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

        [StringLength(100)]
        public string? Organization { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AmountPaid { get; set; }

        [StringLength(50)]
        public string PaymentMethod { get; set; } = string.Empty;

        [StringLength(100)]
        public string? TransactionId { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled

        public string? Notes { get; set; }

        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

        public DateTime? ConfirmedAt { get; set; }
    }
}
