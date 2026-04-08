using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class Policy
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(200)]
        public string? TitleBn { get; set; }

        [StringLength(50)]
        public string Icon { get; set; } = "fas fa-file-alt";

        public string? Description { get; set; }
        public string? DescriptionBn { get; set; }

        public string? PdfUrl { get; set; }

        public int DisplayOrder { get; set; } = 0;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
