namespace FUNewsManagementSystem.API.DTOs.Auth
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public short? AccountId { get; set; }
        public string? AccountName { get; set; }
        public DateTime Expiration { get; set; }
    }
}