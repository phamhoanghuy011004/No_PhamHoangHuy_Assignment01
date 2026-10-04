namespace FUNewsManagementSystem.API.DTOs.Accounts
{
    public class AccountResponseDto
    {
        public short AccountID { get; set; }
        public string? AccountName { get; set; }
        public string? AccountEmail { get; set; }
        public int? AccountRole { get; set; }
        // Không trả về Password
    }
}