using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class PaymentMethod
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? NameBn { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        public string? Icon { get; set; }

        public bool IsActive { get; set; } = true;
        public int DisplayOrder { get; set; }
    }
}
