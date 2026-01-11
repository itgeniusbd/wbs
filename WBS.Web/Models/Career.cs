using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class Career
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

        public string? Department { get; set; }
        public string? Location { get; set; }

        public string? JobType { get; set; } // Full-time, Part-time, Contract

        public string? Description { get; set; }
        public string? DescriptionBn { get; set; }

        public string? Requirements { get; set; }
        public string? RequirementsBn { get; set; }

        public string? Benefits { get; set; }

        public string? SalaryRange { get; set; }

        public DateTime? Deadline { get; set; }

        public string? ApplicationUrl { get; set; }
        public string? ApplicationEmail { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
