using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WBS.Web.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string AccountName { get; set; } = string.Empty;

        [StringLength(200)]
        public string? AccountNameBn { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal AccountBalance { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total_IN { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total_OUT { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total_Income { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total_Expense { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Deleted_Income { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Deleted_Expense { get; set; } = 0;

        public bool Default_Status { get; set; } = false;

        public bool IsActive { get; set; } = true;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(500)]
        public string? DescriptionBn { get; set; }

        [StringLength(50)]
        public string? AccountType { get; set; } // Cash, Bank, Mobile Banking, etc.

        [StringLength(100)]
        public string? AccountNumber { get; set; }

        [StringLength(200)]
        public string? BankName { get; set; }

        [StringLength(200)]
        public string? BranchName { get; set; }

        public int DisplayOrder { get; set; }

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime AccountCreateDate { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public ICollection<OtherIncome> OtherIncomes { get; set; } = new List<OtherIncome>();
        public ICollection<GeneralExpense> GeneralExpenses { get; set; } = new List<GeneralExpense>();
        public ICollection<AccountTransaction> AccountTransactions { get; set; } = new List<AccountTransaction>();
    }
}
