using FUNewsManagementSystem.API.DTOs.Auth;

namespace FUNewsManagementSystem.API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponseDto?> LoginAsync(LoginDto loginDto, CancellationToken cancellationToken = default);
    }
}