using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class District
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? NameBn { get; set; }

        public bool HasWork { get; set; } = false;

        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        public int DisplayOrder { get; set; }

        public ICollection<Upazila> Upazilas { get; set; } = new List<Upazila>();

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
