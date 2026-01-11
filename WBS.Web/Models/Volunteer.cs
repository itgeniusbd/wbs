using System.ComponentModel.DataAnnotations;

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
        public string? City { get; set; }
        public string? Country { get; set; }

        public string? Occupation { get; set; }
        public string? Skills { get; set; }

        public string? Availability { get; set; } // Part-time, Full-time, Weekends

        public string? Message { get; set; }

        public VolunteerStatus Status { get; set; } = VolunteerStatus.Pending;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    public enum VolunteerStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2
    }
}
