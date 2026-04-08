using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    [Index(nameof(Name), nameof(DistrictId), IsUnique = true)]
    public class Upazila
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? NameBn { get; set; }

        [Required]
        public int DistrictId { get; set; }
        public District District { get; set; } = null!;

        public bool HasWork { get; set; } = false;

        public int DisplayOrder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
