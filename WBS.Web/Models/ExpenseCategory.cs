using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class ExpenseCategory
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;

        [StringLength(200)]
        public string? NameBn { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public int DisplayOrder { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<GeneralExpense> GeneralExpenses { get; set; } = new List<GeneralExpense>();
    }
}
