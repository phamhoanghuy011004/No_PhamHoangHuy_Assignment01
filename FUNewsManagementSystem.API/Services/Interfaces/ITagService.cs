using FUNewsManagementSystem.API.Models.Entities;

namespace FUNewsManagementSystem.API.Services.Interfaces
{
    public interface ITagService
    {
        Task<IEnumerable<Tag>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Tag?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Tag>> SearchAsync(string? keyword, CancellationToken cancellationToken = default);
    }
}