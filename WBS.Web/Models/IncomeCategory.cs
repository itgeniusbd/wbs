using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class IncomeCategory
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? NameBn { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? DescriptionBn { get; set; }

        public bool IsActive { get; set; } = true;
        
        public int DisplayOrder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<OtherIncome> OtherIncomes { get; set; } = new List<OtherIncome>();
    }
}
