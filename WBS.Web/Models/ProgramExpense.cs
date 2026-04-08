using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WBS.Web.Models
{
    public class ProgramExpense
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please select an SDG")]
        public int SDGId { get; set; }
        public SDG? SDG { get; set; }

        [Required(ErrorMessage = "Please select a program")]
        public int ProgramId { get; set; }
        public SDGProgram? Program { get; set; }

        public int? ProjectId { get; set; }
        public SDGProject? Project { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, 999999999, ErrorMessage = "Amount must be greater than 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Please select an account")]
        public int AccountId { get; set; }
        public Account? Account { get; set; }

        [Required]
        public DateTime ExpenseDate { get; set; } = DateTime.UtcNow;

        [Required(ErrorMessage = "Details are required")]
        [StringLength(1000)]
        public string Details { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
    }
}

