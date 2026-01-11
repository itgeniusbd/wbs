using System.ComponentModel.DataAnnotations;

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

        public string? Description { get; set; }
        public string? DescriptionBn { get; set; }

        public string? Logo { get; set; }
        public string? Website { get; set; }

        public string? PartnerType { get; set; } // Partner, Sponsor, etc.

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
