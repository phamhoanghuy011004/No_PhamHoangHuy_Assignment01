using FUNewsManagementSystem.API.Models.Entities;

namespace FUNewsManagementSystem.API.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        Task<IEnumerable<Category>> SearchAsync(string? keyword, CancellationToken cancellationToken = default);
        Task<bool> IsUsedByAnyNewsAsync(short categoryId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);
    }
}