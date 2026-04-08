using System.ComponentModel.DataAnnotations;

namespace WBS.Web.ViewModels
{
    public class RoleListViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int UserCount { get; set; }
        public int PermissionCount { get; set; }
    }

    public class CreateRoleViewModel
    {
        [Required(ErrorMessage = "Role name is required")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public List<int> SelectedPermissions { get; set; } = new();
    }

    public class EditRoleViewModel
    {
        public string Id { get; set; } = string.Empty;

        [Required(ErrorMessage = "Role name is required")]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public List<int> SelectedPermissions { get; set; } = new();
    }

    public class RolePermissionsViewModel
    {
        public string RoleId { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public List<PermissionGroupViewModel> PermissionGroups { get; set; } = new();
    }

    public class PermissionGroupViewModel
    {
        public string Module { get; set; } = string.Empty;
        public List<PermissionViewModel> Permissions { get; set; } = new();
    }

    public class PermissionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string NameBn { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public bool IsGranted { get; set; }
    }
}
