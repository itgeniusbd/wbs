using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WBS.Web.Models
{
    public class News
    {
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Title { get; set; } = string.Empty;

        [StringLength(300)]
        public string? TitleBn { get; set; }

        [StringLength(300)]
        public string Slug { get; set; } = string.Empty;

        public string? Summary { get; set; }
        public string? SummaryBn { get; set; }

        public string? Content { get; set; }
        public string? ContentBn { get; set; }

        public string? FeaturedImage { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;

        public int ViewCount { get; set; } = 0;

        public DateTime PublishedDate { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [NotMapped]
        public DateTime PublishedAt
        {
            get => PublishedDate;
            set => PublishedDate = value;
        }
    }
}
