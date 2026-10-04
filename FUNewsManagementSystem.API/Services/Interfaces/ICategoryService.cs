using FUNewsManagementSystem.API.Models.Entities;

namespace FUNewsManagementSystem.API.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<Category?> GetByIdAsync(short id, CancellationToken cancellationToken = default);
        Task<IEnumerable<Category>> SearchAsync(string? keyword, CancellationToken cancellationToken = default);
        Task<IEnumerable<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default);
        Task<(bool Success, string Message)> CreateAsync(Category category, CancellationToken cancellationToken = default);
        Task<(bool Success, string Message)> UpdateAsync(Category category, CancellationToken cancellationToken = default);
        Task<(bool Success, string Message)> DeleteAsync(short id, CancellationToken cancellationToken = default);
    }
}