using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class AnnualReport
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(200)]
        public string? TitleBn { get; set; }

        public int Year { get; set; }

        public string? Description { get; set; }
        public string? DescriptionBn { get; set; }

        public string? CoverImage { get; set; }

        [Required]
        public string FileUrl { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
