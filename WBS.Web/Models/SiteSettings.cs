using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    public class SiteSettings
    {
        public int Id { get; set; }

        [StringLength(100)]
        public string SiteName { get; set; } = "WBS";

        [StringLength(100)]
        public string? SiteNameBn { get; set; }

        public string? Logo { get; set; }
        public string? LogoWhite { get; set; }
        public string? Favicon { get; set; }

        public string? AboutUsImage { get; set; }

        public string? AboutUs { get; set; }
        public string? AboutUsBn { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        public string? Address { get; set; }
        public string? AddressBn { get; set; }

        public string? FacebookUrl { get; set; }
        public string? TwitterUrl { get; set; }
        public string? InstagramUrl { get; set; }
        public string? YouTubeUrl { get; set; }
        public string? LinkedInUrl { get; set; }

        public string? GoogleMapEmbed { get; set; }

        public string? FooterText { get; set; }
        public string? FooterTextBn { get; set; }

        public string? CopyrightText { get; set; }

        // Bank Details
        public string? BankName { get; set; }
        public string? BankAccountName { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? BankBranchName { get; set; }
        public string? BankRoutingNumber { get; set; }
        public string? BankSwiftCode { get; set; }

        // Mobile Banking
        public string? BkashNumber { get; set; }
        public string? NagadNumber { get; set; }
        public string? RocketNumber { get; set; }

        // Meta
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? MetaKeywords { get; set; }

        // Organization Information
        [StringLength(200)]
        public string? OrganizationFullName { get; set; }
        
        [StringLength(200)]
        public string? OrganizationFullNameBn { get; set; }
        
        [StringLength(100)]
        public string? RegistrationNumber { get; set; }
        
        [StringLength(100)]
        public string? RegistrationType { get; set; } // RJSC, NGO Bureau, etc.
        
        public int? EstablishedYear { get; set; }
        
        public string? OrganizationType { get; set; }
        public string? OrganizationTypeBn { get; set; }
        
        public string? ManagementInfo { get; set; }
        public string? ManagementInfoBn { get; set; }

        // Refund & Return Policy
        [StringLength(500)]
        public string? RefundPolicyTimeframe { get; set; }
        
        [StringLength(500)]
        public string? RefundPolicyTimeframeBn { get; set; }

        // Payment Gateway Banner
        public string? PaymentGatewayBanner { get; set; }

        // Rohingya Statistics (Manual Override)
        public int? RohingyaCampsReached { get; set; }
        public int? RohingyaTotalBeneficiaries { get; set; }
        public int? RohingyaActivePrograms { get; set; }
        public int? RohingyaEventsConducted { get; set; }

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
