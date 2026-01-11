using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WBS.Web.Models
{
    public class Menu
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? NameBn { get; set; }

        [StringLength(200)]
        public string? Url { get; set; }

        public int? ParentMenuId { get; set; }
        public Menu? ParentMenu { get; set; }
        public ICollection<Menu> SubMenus { get; set; } = new List<Menu>();

        public int? PageId { get; set; }
        public Page? Page { get; set; }

        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsExternal { get; set; } = false;
        public string? Icon { get; set; }
        public string? CssClass { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
