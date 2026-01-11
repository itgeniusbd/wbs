using Microsoft.AspNetCore.Identity;

namespace WBS.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? FirstNameBn { get; set; }
        public string? LastNameBn { get; set; }
        public string? ProfilePicture { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? LastLoginAt { get; set; }
    }
}
