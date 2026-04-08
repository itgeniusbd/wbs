using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WBS.Web.Models
{
    public class Publication
    {
        public int Id { get; set; }

        [Required]
        [StringLength(300)]
        public string Title { get; set; } = string.Empty;

        [StringLength(300)]
        public string? TitleBn { get; set; }

        public string? Description { get; set; }
        public string? DescriptionBn { get; set; }

        [StringLength(200)]
        public string? Author { get; set; }

        [StringLength(200)]
        public string? AuthorBn { get; set; }

        [StringLength(200)]
        public string? Publisher { get; set; }

        [StringLength(200)]
        public string? PublisherBn { get; set; }

        public string? Tags { get; set; }

        public string? CoverImage { get; set; }
        public string? FileUrl { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime? PublishedDate { get; set; }
    }
}
