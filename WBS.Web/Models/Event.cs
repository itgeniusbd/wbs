using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WBS.Web.Models
{
    public class Event
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

        public string? Description { get; set; }
        public string? DescriptionBn { get; set; }

        public string? FeaturedImage { get; set; }

        public string? Location { get; set; }
        public string? LocationBn { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? TicketPrice { get; set; }

        public int? TotalCapacity { get; set; }

        public DateTime? RegistrationDeadline { get; set; }

        public string? RegistrationUrl { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsFeatured { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<EventRegistration> Registrations { get; set; } = new List<EventRegistration>();

        [NotMapped]
        public int RegisteredCount => Registrations?.Count(r => r.Status == "Confirmed") ?? 0;

        [NotMapped]
        public int AvailableSeats => TotalCapacity.HasValue ? TotalCapacity.Value - RegisteredCount : 0;

        [NotMapped]
        public bool IsFull => TotalCapacity.HasValue && RegisteredCount >= TotalCapacity.Value;

        [NotMapped]
        public bool IsRegistrationOpen => 
            IsActive && 
            !IsFull && 
            (RegistrationDeadline == null || RegistrationDeadline > DateTime.UtcNow) &&
            StartDate > DateTime.UtcNow;
    }
}
