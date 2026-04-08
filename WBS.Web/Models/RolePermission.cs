using Microsoft.AspNetCore.Identity;

namespace WBS.Web.Models
{
    public class RolePermission
    {
        public string RoleId { get; set; } = string.Empty;
        public int PermissionId { get; set; }
        public DateTime GrantedAt { get; set; } = DateTime.UtcNow;
        
        public virtual IdentityRole Role { get; set; } = null!;
        public virtual Permission Permission { get; set; } = null!;
    }
}
