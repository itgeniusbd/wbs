using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class CVApplication
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(200)]
        public string? Education { get; set; }

        [StringLength(200)]
        public string? Experience { get; set; }

        public string? Skills { get; set; }

        public string? CVFilePath { get; set; }

        public string? CoverLetter { get; set; }

        public int? CareerIdAppliedFor { get; set; }

        [StringLength(200)]
        public string? PositionAppliedFor { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        public DateTime AppliedDate { get; set; } = DateTime.UtcNow;

        public DateTime? ReviewedDate { get; set; }

        public string? ReviewNotes { get; set; }
    }
}
