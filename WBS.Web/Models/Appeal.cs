using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace WBS.Web.Models
{
    public class Appeal
    {
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Title { get; set; } = string.Empty;

        [StringLength(300)]
        public string? TitleBn { get; set; }

        [Required]
        [StringLength(300)]
        public string Slug { get; set; } = string.Empty;

        public string? Summary { get; set; }
        public string? SummaryBn { get; set; }

        public string? Content { get; set; }
        public string? ContentBn { get; set; }

        public string? FeaturedImage { get; set; }
        public string? BannerImage { get; set; }

        public decimal? TargetAmount { get; set; }
        public decimal RaisedAmount { get; set; } = 0;

        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public string? AppealType { get; set; } // Emergency, Special, Regular

        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;
        public bool IsUrgent { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [JsonIgnore]
        public ICollection<Donation> Donations { get; set; } = new List<Donation>();

        [NotMapped]
        public int DonationPercentage => TargetAmount.HasValue && TargetAmount > 0 
            ? (int)((RaisedAmount / TargetAmount.Value) * 100) 
            : 0;
    }
}
