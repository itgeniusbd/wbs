using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class Page
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(200)]
        public string? TitleBn { get; set; }

        [Required]
        [StringLength(200)]
        public string Slug { get; set; } = string.Empty;

        public string? Content { get; set; }
        public string? ContentBn { get; set; }

        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }

        public string? FeaturedImage { get; set; }
        public string? BannerImage { get; set; }

        public bool IsActive { get; set; } = true;
        public bool ShowInFooter { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }
    }
}
