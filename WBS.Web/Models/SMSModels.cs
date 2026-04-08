using System.ComponentModel.DataAnnotations;

namespace WBS.Web.Models
{
    // Contact Group for organizing contacts
    public class ContactGroup
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string GroupName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ContactListItem> Contacts { get; set; } = new List<ContactListItem>();
    }

    // Contact List Item
    public class ContactListItem
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Email { get; set; }

        [StringLength(50)]
        public string? Type { get; set; } // Donor, Volunteer, General, etc.

        public int? ContactGroupId { get; set; }
        public ContactGroup? ContactGroup { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }

    // SMS Campaign for tracking bulk SMS
    public class SMSCampaign
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string CampaignName { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        [StringLength(50)]
        public string RecipientType { get; set; } = string.Empty; // AllDonors, SelectedDonors, AllVolunteers, ContactGroup, etc.

        public int? ContactGroupId { get; set; }
        public ContactGroup? ContactGroup { get; set; }

        public int TotalRecipients { get; set; }
        public int SuccessCount { get; set; }
        public int FailedCount { get; set; }

        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Sending, Completed, Failed

        [StringLength(100)]
        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SentAt { get; set; }

        public ICollection<SMSCampaignRecipient> Recipients { get; set; } = new List<SMSCampaignRecipient>();
    }

    // Individual recipient in a campaign
    public class SMSCampaignRecipient
    {
        public int Id { get; set; }

        public int SMSCampaignId { get; set; }
        public SMSCampaign? SMSCampaign { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, Sent, Failed

        public DateTime? SentAt { get; set; }

        [StringLength(500)]
        public string? ErrorMessage { get; set; }
    }
}
