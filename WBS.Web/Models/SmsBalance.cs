using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class SmsBalance
    {
        public int Id { get; set; }

        [Required]
        public int AvailableBalance { get; set; }

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        [StringLength(200)]
        public string? UpdatedBy { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
