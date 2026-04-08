namespace WBS.Web.Models
{
    public class Permission
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NameBn { get; set; } = string.Empty;
        public string Module { get; set; } = string.Empty; // e.g., "Donations", "Users", "Pages"
        public string Action { get; set; } = string.Empty; // e.g., "View", "Create", "Edit", "Delete"
        public string? Description { get; set; }
        public int DisplayOrder { get; set; }
        
        public virtual ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();
    }
}
