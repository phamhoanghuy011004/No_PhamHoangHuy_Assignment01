using FUNewsManagementSystem.API.Models.Entities;

namespace FUNewsManagementSystem.API.Services.Interfaces
{
    public interface INewsArticleService
    {
        Task<IEnumerable<NewsArticle>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<NewsArticle?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<NewsArticle>> GetActiveNewsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<NewsArticle>> GetNewsByCreatorAsync(short creatorId, CancellationToken cancellationToken = default);
        Task<IEnumerable<NewsArticle>> SearchAsync(string? keyword, CancellationToken cancellationToken = default);
        Task<IEnumerable<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<(bool Success, string Message)> CreateAsync(NewsArticle news, List<int>? tagIds, CancellationToken cancellationToken = default);
        Task<(bool Success, string Message)> UpdateAsync(NewsArticle news, List<int>? tagIds, CancellationToken cancellationToken = default);
        Task<(bool Success, string Message)> DeleteAsync(string id, CancellationToken cancellationToken = default);
    }
}