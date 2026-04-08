using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WBS.Web.Models
{
    public class LegalStatus
    {
        public int Id { get; set; }

        [Required]
        [StringLength(500)]
        public string CertificateImage { get; set; } = string.Empty;

        [StringLength(1000)]
        public string? CertificateImageBn { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<RegistrationInfo> RegistrationInfos { get; set; } = new List<RegistrationInfo>();
    }

    public class RegistrationInfo
    {
        public int Id { get; set; }

        public int LegalStatusId { get; set; }

        [Required]
        [StringLength(300)]
        public string Authority { get; set; } = string.Empty;

        [StringLength(300)]
        public string? AuthorityBn { get; set; }

        [Required]
        [StringLength(200)]
        public string RegistrationNumber { get; set; } = string.Empty;

        [StringLength(200)]
        public string? RegistrationNumberBn { get; set; }

        [Required]
        public DateTime RegistrationDate { get; set; }

        public int DisplayOrder { get; set; }

        public bool IsActive { get; set; } = true;

        [ValidateNever]
        public LegalStatus LegalStatus { get; set; } = null!;
    }
}
