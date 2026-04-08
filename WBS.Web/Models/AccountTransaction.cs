using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WBS.Web.Models
{
    public class AccountTransaction
    {
        public int Id { get; set; }

        [Required]
        public int AccountId { get; set; }
        public Account? Account { get; set; }

        [Required]
        [StringLength(50)]
        public string TransactionType { get; set; } = string.Empty; // Income, Expense, Transfer_In, Transfer_Out

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BalanceBefore { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal BalanceAfter { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? ReferenceType { get; set; } // OtherIncome, GeneralExpense, Donation, Transfer

        public int? ReferenceId { get; set; }

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
