using FUNewsManagementSystem.API.Data;
using FUNewsManagementSystem.API.Models.Entities;
using FUNewsManagementSystem.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.API.Repositories
{
    public class NewsArticleRepository : GenericRepository<NewsArticle>, INewsArticleRepository
    {
        public NewsArticleRepository(FUNewsManagementDbContext context) : base(context)
        {
        }

        public async Task<NewsArticle?> GetByIdWithDetailsAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .FirstOrDefaultAsync(n => n.NewsArticleID == id, cancellationToken);
        }

        public async Task<IEnumerable<NewsArticle>> GetActiveNewsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .Where(n => n.NewsStatus == true)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByCreatorAsync(short creatorId, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .Where(n => n.CreatedByID == creatorId)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<NewsArticle>> SearchAsync(string? keyword, CancellationToken cancellationToken = default)
        {
            var query = _dbSet
                .Include(n => n.Category)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim().ToLower();
                query = query.Where(n =>
                    (n.NewsTitle != null && n.NewsTitle.ToLower().Contains(keyword)) ||
                    n.Headline.ToLower().Contains(keyword) ||
                    (n.NewsContent != null && n.NewsContent.ToLower().Contains(keyword)));
            }

            return await query
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Where(n => n.CreatedDate >= startDate && n.CreatedDate <= endDate)
                .OrderByDescending(n => n.CreatedDate)
                .ToListAsync(cancellationToken);
        }
    }
}