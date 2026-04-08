using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WBS.Web.Models
{
    public class Partner
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? NameBn { get; set; }

        [Column("LogoUrl")]
        public string? Logo { get; set; }
        
        [Url]
        public string? Website { get; set; }

        [Url]
        public string? FacebookUrl { get; set; }
        
        [Url]
        public string? TwitterUrl { get; set; }
        
        [Url]
        public string? LinkedInUrl { get; set; }
        
        [Url]
        public string? InstagramUrl { get; set; }

        [Url]
        public string? YouTubeUrl { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;

        [NotMapped]
        public string? Description { get; set; }        
        
        [NotMapped]
        public string? DescriptionBn { get; set; }

        [NotMapped]
        [StringLength(100)]
        public string? PartnerType { get; set; }

        [NotMapped]
        [EmailAddress]
        public string? Email { get; set; }

        [NotMapped]
        [Phone]
        public string? Phone { get; set; }

        [NotMapped]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        [NotMapped]
        public DateTime? UpdatedAt { get; set; }
    }
}
