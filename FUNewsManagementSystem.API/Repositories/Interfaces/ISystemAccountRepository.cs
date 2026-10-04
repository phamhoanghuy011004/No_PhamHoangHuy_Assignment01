using FUNewsManagementSystem.API.Models.Entities;

namespace FUNewsManagementSystem.API.Repositories.Interfaces
{
    public interface ISystemAccountRepository : IGenericRepository<SystemAccount>
    {
        Task<SystemAccount?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<IEnumerable<SystemAccount>> SearchAsync(string? keyword, CancellationToken cancellationToken = default);
        Task<bool> HasCreatedAnyNewsAsync(short accountId, CancellationToken cancellationToken = default);
    }
}