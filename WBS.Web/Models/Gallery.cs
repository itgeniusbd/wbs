using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class Gallery
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(200)]
        public string? TitleBn { get; set; }

        public string? Description { get; set; }
        public string? DescriptionBn { get; set; }

        public string? CoverImage { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<GalleryImage> Images { get; set; } = new List<GalleryImage>();
    }

    public class GalleryImage
    {
        public int Id { get; set; }

        public int GalleryId { get; set; }
        public Gallery? Gallery { get; set; }

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        public string? Caption { get; set; }
        public string? CaptionBn { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
