using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class Story
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

        public string? BeneficiaryName { get; set; }
        public string? BeneficiaryNameBn { get; set; }

        public string? Summary { get; set; }
        public string? SummaryBn { get; set; }

        public string? Content { get; set; }
        public string? ContentBn { get; set; }

        public string? FeaturedImage { get; set; }
        public string? VideoUrl { get; set; }

        public string? Location { get; set; }
        public string? LocationBn { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
