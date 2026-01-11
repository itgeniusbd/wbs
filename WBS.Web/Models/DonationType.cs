using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WBS.Web.Models
{
    public class DonationType
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? NameBn { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? DescriptionBn { get; set; }

        public string? Icon { get; set; }
        public string? Image { get; set; }

        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }

        [JsonIgnore]
        public ICollection<Donation> Donations { get; set; } = new List<Donation>();
    }
}
