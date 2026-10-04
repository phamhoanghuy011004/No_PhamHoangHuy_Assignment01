using FUNewsManagementSystem.API.Models.Entities;

namespace FUNewsManagementSystem.API.Repositories.Interfaces
{
    public interface INewsArticleRepository : IGenericRepository<NewsArticle>
    {
        Task<NewsArticle?> GetByIdWithDetailsAsync(string id, CancellationToken cancellationToken = default);
        Task<IEnumerable<NewsArticle>> GetActiveNewsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<NewsArticle>> GetNewsByCreatorAsync(short creatorId, CancellationToken cancellationToken = default);
        Task<IEnumerable<NewsArticle>> SearchAsync(string? keyword, CancellationToken cancellationToken = default);
        Task<IEnumerable<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
    }
}