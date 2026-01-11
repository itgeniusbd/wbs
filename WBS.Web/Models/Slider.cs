using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class Slider
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(200)]
        public string? TitleBn { get; set; }

        [StringLength(500)]
        public string? Subtitle { get; set; }

        [StringLength(500)]
        public string? SubtitleBn { get; set; }

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        public string? ButtonText { get; set; }
        public string? ButtonTextBn { get; set; }
        public string? ButtonUrl { get; set; }

        public string? SecondButtonText { get; set; }
        public string? SecondButtonUrl { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
