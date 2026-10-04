using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Web.ViewModels
{
    public class ProfileViewModel
    {
        public short AccountID { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string AccountName { get; set; } = string.Empty;

        [EmailAddress]
        [Display(Name = "Email")]
        public string AccountEmail { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        [Display(Name = "New Password (leave blank to keep current)")]
        public string? AccountPassword { get; set; }
    }
}