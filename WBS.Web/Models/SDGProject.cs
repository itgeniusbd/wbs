using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class SDGProject
    {
        public int Id { get; set; }

        [Required]
        public int SDGId { get; set; }
        public SDG? SDG { get; set; }

        // Program Relationship (New)
        public int? SDGProgramId { get; set; }
        public SDGProgram? SDGProgram { get; set; }

        [Required]
        [StringLength(300)]
        public string Title { get; set; } = string.Empty;

        [StringLength(300)]
        public string? TitleBn { get; set; }

        public string? Description { get; set; }
        public string? DescriptionBn { get; set; }

        [StringLength(100)]
        public string? District { get; set; }

        [StringLength(100)]
        public string? DistrictBn { get; set; }

        [StringLength(100)]
        public string? Thana { get; set; }

        [StringLength(100)]
        public string? ThanaBn { get; set; }

        [StringLength(100)]
        public string? Union { get; set; }

        [StringLength(100)]
        public string? UnionBn { get; set; }

        [StringLength(100)]
        public string? Village { get; set; }

        [StringLength(100)]
        public string? VillageBn { get; set; }

        public int BeneficiaryCount { get; set; }

        public string? FeaturedImage { get; set; }

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<SDGProjectImage> Images { get; set; } = new List<SDGProjectImage>();
    }

    public class SDGProjectImage
    {
        public int Id { get; set; }

        [Required]
        public int SDGProjectId { get; set; }
        public SDGProject? SDGProject { get; set; }

        [Required]
        public string ImageUrl { get; set; } = string.Empty;

        [StringLength(200)]
        public string? Caption { get; set; }

        [StringLength(200)]
        public string? CaptionBn { get; set; }

        public int DisplayOrder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
