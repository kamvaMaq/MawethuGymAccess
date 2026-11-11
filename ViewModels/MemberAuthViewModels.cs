using System.ComponentModel.DataAnnotations;

namespace MawethuGym.Web.ViewModels
{
    public class MemberRegisterViewModel
    {
        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [Phone]
        public string Phone { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Emergency Contact Number")]
        public string EmergencyContact { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Membership Plan")]
        public string PlanType { get; set; } = "1 Month"; // 1 Month R260, 3 Months R480, 6 Months R900[cite: 2]

        [Required]
        [Display(Name = "Biometric Consent")]
        public bool BiometricConsentGiven { get; set; } // POPIA compliance requirement[cite: 5]
    }

    public class MemberLoginViewModel
    {
        [Required]
        [Display(Name = "Email or Phone")]
        public string Identifier { get; set; } = string.Empty;
    }
}