using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? Phone { get; set; }

        [StringLength(200)]
        public string? Subject { get; set; }

        [Required]
        public string Message { get; set; } = string.Empty;

        public bool IsRead { get; set; } = false;
        public bool IsReplied { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
