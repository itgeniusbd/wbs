using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WBS.Web.Models
{
    public class Volunteer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? Phone { get; set; }

        public string? Address { get; set; }
        public string? Skills { get; set; }

        public int? SDGProjectId { get; set; }
        
        [ForeignKey("SDGProjectId")]
        public SDGProject? SDGProject { get; set; }

        public string? Message { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Pending";

        public DateTime AppliedDate { get; set; } = DateTime.UtcNow;
    }

    public enum VolunteerStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }
}
