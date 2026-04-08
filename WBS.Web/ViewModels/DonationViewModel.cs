using System.ComponentModel.DataAnnotations;

namespace WBS.Web.ViewModels
{
    public class DonationViewModel
    {
        [Required(ErrorMessage = "??? ??????")]
        [Display(Name = "Full Name")]
        public string DonorName { get; set; } = string.Empty;

        [Required(ErrorMessage = "????? ??????")]
        [EmailAddress(ErrorMessage = "???? ????? ???")]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string? Phone { get; set; }

        public string? Address { get; set; }

        [Required(ErrorMessage = "?????? ??????")]
        [Range(10, 10000000, ErrorMessage = "???? ?????? ???")]
        public decimal Amount { get; set; }

        public string Currency { get; set; } = "BDT";

        [Required(ErrorMessage = "?????? ???? ???????? ????")]
        public int DonationTypeId { get; set; }

        public int? AppealId { get; set; }

        [Required(ErrorMessage = "??????? ???? ???????? ????")]
        public string PaymentMethod { get; set; } = string.Empty;

        public bool IsRecurring { get; set; } = false;
        public string? RecurringFrequency { get; set; }

        public bool IsAnonymous { get; set; } = false;
        public string? Notes { get; set; }

        public List<DonationTypeViewModel> DonationTypes { get; set; } = new();
        public List<AppealViewModel> Appeals { get; set; } = new();
    }

    public class QuickDonateViewModel
    {
        public string DonationFrequency { get; set; } = "one-off"; // one-off, monthly
        public int DonationTypeId { get; set; }
        public decimal Amount { get; set; }
    }
}
