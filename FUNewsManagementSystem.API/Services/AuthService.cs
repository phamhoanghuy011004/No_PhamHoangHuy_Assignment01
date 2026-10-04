using FUNewsManagementSystem.API.DTOs.Auth;
using FUNewsManagementSystem.API.Repositories.Interfaces;
using FUNewsManagementSystem.API.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FUNewsManagementSystem.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly ISystemAccountRepository _accountRepository;
        private readonly IConfiguration _configuration;

        public AuthService(ISystemAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default)
        {
            // 1. Kiểm tra Admin từ appsettings.json trước
            var adminEmail = _configuration["AdminAccount:Email"];
            var adminPassword = _configuration["AdminAccount:Password"];

            if (!string.IsNullOrEmpty(adminEmail) &&
                !string.IsNullOrEmpty(adminPassword) &&
                loginDto.Email.Equals(adminEmail, StringComparison.OrdinalIgnoreCase) &&
                loginDto.Password == adminPassword)
            {
                return GenerateToken(
                    email: adminEmail,
                    role: "Admin",
                    accountId: null,
                    accountName: "Administrator");
            }

            // 2. Kiểm tra tài khoản trong Database
            var account = await _accountRepository.GetByEmailAsync(loginDto.Email, cancellationToken);

            if (account == null || account.AccountPassword != loginDto.Password)
            {
                return null; // Login failed
            }

            // Map Role
            string role = account.AccountRole switch
            {
                1 => "Staff",
                2 => "Lecturer",
                _ => "Lecturer"
            };

            return GenerateToken(
                email: account.AccountEmail ?? string.Empty,
                role: role,
                accountId: account.AccountID,
                accountName: account.AccountName);
        }

        private LoginResponseDto GenerateToken(string email, string role, short? accountId, string? accountName)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("Jwt:Key is missing");
            var jwtIssuer = _configuration["Jwt:Issuer"] ?? "FUNewsManagementSystem";
            var jwtAudience = _configuration["Jwt:Audience"] ?? "FUNewsManagementSystemClient";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (accountId.HasValue)
            {
                claims.Add(new Claim(ClaimTypes.NameIdentifier, accountId.Value.ToString()));
            }

            if (!string.IsNullOrEmpty(accountName))
            {
                claims.Add(new Claim(ClaimTypes.Name, accountName));
            }

            var expiration = DateTime.UtcNow.AddHours(8);

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAudience,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new LoginResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Email = email,
                Role = role,
                AccountId = accountId,
                AccountName = accountName,
                Expiration = expiration
            };
        }
    }
}