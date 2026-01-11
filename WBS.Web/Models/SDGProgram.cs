using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class SDGProgram
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(200)]
        public string? TitleBn { get; set; }

        public string? Description { get; set; }

        public string? DescriptionBn { get; set; }

        [StringLength(500)]
        public string? FeaturedImage { get; set; }

        // SDG Relationship
        public int SDGId { get; set; }
        public SDG? SDG { get; set; }

        // Events under this program (previously SDGProjects)
        public ICollection<SDGProject> Events { get; set; } = new List<SDGProject>();

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsFeatured { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        [StringLength(100)]
        public string? CreatedBy { get; set; }
    }
}
