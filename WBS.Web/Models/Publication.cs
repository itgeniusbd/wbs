using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class Publication
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

        public string? CoverImage { get; set; }
        public string? FileUrl { get; set; }

        public string? PublicationType { get; set; } // Blog, Research, Magazine, etc.

        public bool IsActive { get; set; } = true;

        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
