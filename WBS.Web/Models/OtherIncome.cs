using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WBS.Web.Models
{
    public class OtherIncome
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, 999999999, ErrorMessage = "Amount must be greater than 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        public int IncomeCategoryId { get; set; }
        public IncomeCategory? IncomeCategory { get; set; }

        [Required(ErrorMessage = "Please select an account")]
        public int AccountId { get; set; }
        public Account? Account { get; set; }

        [Required]
        public DateTime IncomeDate { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
    }
}
