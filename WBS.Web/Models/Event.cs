using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class Event
    {
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Title { get; set; } = string.Empty;

        [StringLength(300)]
        public string? TitleBn { get; set; }

        [Required]
        [StringLength(300)]
        public string Slug { get; set; } = string.Empty;

        public string? Description { get; set; }
        public string? DescriptionBn { get; set; }

        public string? FeaturedImage { get; set; }

        public string? Location { get; set; }
        public string? LocationBn { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? RegistrationUrl { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
