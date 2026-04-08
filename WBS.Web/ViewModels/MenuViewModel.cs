using System.ComponentModel.DataAnnotations;

namespace WBS.Web.ViewModels
{
    public class MenuViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? NameEn { get; set; }
        public string? NameBn { get; set; }
        public string? Url { get; set; }
        public int? PageId { get; set; }
        public string? Icon { get; set; }
        public string? CssClass { get; set; }
        public bool IsExternal { get; set; }
        public List<MenuViewModel> SubMenus { get; set; } = new();
    }

    public class MenuCreateViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? NameBn { get; set; }

        [StringLength(200)]
        public string? Url { get; set; }

        public int? ParentMenuId { get; set; }
        public int? PageId { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsExternal { get; set; } = false;

        public string? Icon { get; set; }
        public string? CssClass { get; set; }
    }
}
