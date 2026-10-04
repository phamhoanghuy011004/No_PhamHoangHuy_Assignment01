using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.API.DTOs.Accounts
{
    public class AccountCreateDto
    {
        [Required(ErrorMessage = "AccountID is required")]
        [Range(1, short.MaxValue, ErrorMessage = "AccountID must be greater than 0")]
        public short AccountID { get; set; }

        [Required(ErrorMessage = "AccountName is required")]
        [StringLength(100, ErrorMessage = "AccountName cannot exceed 100 characters")]
        public string AccountName { get; set; } = string.Empty;

        [Required(ErrorMessage = "AccountEmail is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [StringLength(70, ErrorMessage = "AccountEmail cannot exceed 70 characters")]
        public string AccountEmail { get; set; } = string.Empty;

        [Required(ErrorMessage = "AccountRole is required")]
        [Range(1, 2, ErrorMessage = "AccountRole must be 1 (Staff) or 2 (Lecturer)")]
        public int AccountRole { get; set; }

        [Required(ErrorMessage = "AccountPassword is required")]
        [StringLength(70, MinimumLength = 1, ErrorMessage = "AccountPassword must be between 1 and 70 characters")]
        public string AccountPassword { get; set; } = string.Empty;
    }
}