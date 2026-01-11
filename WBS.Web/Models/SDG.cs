using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class SDG
    {
        public int Id { get; set; }

        public int Number { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? NameBn { get; set; }

        public string? Description { get; set; }
        public string? DescriptionBn { get; set; }

        public string? Icon { get; set; }
        public string? Image { get; set; }
        public string? Color { get; set; }

        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }

        public ICollection<Sector> Sectors { get; set; } = new List<Sector>();
    }

    public class Sector
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? NameBn { get; set; }

        public string? Description { get; set; }
        public string? DescriptionBn { get; set; }

        public string? Icon { get; set; }
        public string? Image { get; set; }

        public int? SDGId { get; set; }
        public SDG? SDG { get; set; }

        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
