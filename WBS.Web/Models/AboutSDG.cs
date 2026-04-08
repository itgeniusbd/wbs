using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class AboutSDG
    {
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Title { get; set; } = string.Empty;

        [StringLength(300)]
        public string? TitleBn { get; set; }

        [Required]
        public string Content { get; set; } = string.Empty;

        public string? ContentBn { get; set; }

        [Required]
        public string FeaturedImage { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
