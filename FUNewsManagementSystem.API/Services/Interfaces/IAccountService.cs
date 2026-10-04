using FUNewsManagementSystem.API.Models.Entities;

namespace FUNewsManagementSystem.API.Services.Interfaces
{
    public interface IAccountService
    {
        Task<IEnumerable<SystemAccount>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<SystemAccount?> GetByIdAsync(short id, CancellationToken cancellationToken = default);
        Task<SystemAccount?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<IEnumerable<SystemAccount>> SearchAsync(string? keyword, CancellationToken cancellationToken = default);
        Task<(bool Success, string Message)> CreateAsync(SystemAccount account, CancellationToken cancellationToken = default);
        Task<(bool Success, string Message)> UpdateAsync(SystemAccount account, CancellationToken cancellationToken = default);
        Task<(bool Success, string Message)> DeleteAsync(short id, CancellationToken cancellationToken = default);
    }
}