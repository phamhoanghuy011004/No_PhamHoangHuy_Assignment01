using FUNewsManagementSystem.API.Data;
using FUNewsManagementSystem.API.Models.Entities;
using FUNewsManagementSystem.API.Repositories.Interfaces;
using FUNewsManagementSystem.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.API.Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _newsRepository;
        private readonly ITagRepository _tagRepository;
        private readonly FUNewsManagementDbContext _context;

        public NewsArticleService(
            INewsArticleRepository newsRepository,
            ITagRepository tagRepository,
            FUNewsManagementDbContext context)
        {
            _newsRepository = newsRepository;
            _tagRepository = tagRepository;
            _context = context;
        }

        public async Task<IEnumerable<NewsArticle>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _newsRepository.SearchAsync(null, cancellationToken);
        }

        public async Task<NewsArticle?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
        {
            return await _newsRepository.GetByIdWithDetailsAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<NewsArticle>> GetActiveNewsAsync(CancellationToken cancellationToken = default)
        {
            return await _newsRepository.GetActiveNewsAsync(cancellationToken);
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByCreatorAsync(short creatorId, CancellationToken cancellationToken = default)
        {
            return await _newsRepository.GetNewsByCreatorAsync(creatorId, cancellationToken);
        }

        public async Task<IEnumerable<NewsArticle>> SearchAsync(string? keyword, CancellationToken cancellationToken = default)
        {
            return await _newsRepository.SearchAsync(keyword, cancellationToken);
        }

        public async Task<IEnumerable<NewsArticle>> GetNewsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _newsRepository.GetNewsByDateRangeAsync(startDate, endDate, cancellationToken);
        }

        public async Task<(bool Success, string Message)> CreateAsync(NewsArticle news, List<int>? tagIds, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(news.NewsArticleID))
            {
                return (false, "NewsArticleID is required.");
            }

            if (string.IsNullOrWhiteSpace(news.Headline))
            {
                return (false, "Headline is required.");
            }

            var exists = await _newsRepository.ExistsAsync(n => n.NewsArticleID == news.NewsArticleID, cancellationToken);
            if (exists)
            {
                return (false, "NewsArticleID already exists.");
            }

            news.CreatedDate = DateTime.Now;
            news.ModifiedDate = DateTime.Now;

            await _newsRepository.AddAsync(news, cancellationToken);

            // Handle Tags
            if (tagIds != null && tagIds.Any())
            {
                foreach (var tagId in tagIds.Distinct())
                {
                    var tagExists = await _tagRepository.ExistsAsync(t => t.TagID == tagId, cancellationToken);
                    if (tagExists)
                    {
                        await _context.NewsTags.AddAsync(new NewsTag
                        {
                            NewsArticleID = news.NewsArticleID,
                            TagID = tagId
                        }, cancellationToken);
                    }
                }
            }

            await _newsRepository.SaveChangesAsync(cancellationToken);

            return (true, "News article created successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(NewsArticle news, List<int>? tagIds, CancellationToken cancellationToken = default)
        {
            var existing = await _newsRepository.GetByIdWithDetailsAsync(news.NewsArticleID, cancellationToken);
            if (existing == null)
            {
                return (false, "News article not found.");
            }

            existing.NewsTitle = news.NewsTitle;
            existing.Headline = news.Headline;
            existing.NewsContent = news.NewsContent;
            existing.NewsSource = news.NewsSource;
            existing.CategoryID = news.CategoryID;
            existing.NewsStatus = news.NewsStatus;
            existing.UpdatedByID = news.UpdatedByID;
            existing.ModifiedDate = DateTime.Now;

            // Remove old tags
            var oldTags = _context.NewsTags.Where(nt => nt.NewsArticleID == existing.NewsArticleID);
            _context.NewsTags.RemoveRange(oldTags);

            // Add new tags
            if (tagIds != null && tagIds.Any())
            {
                foreach (var tagId in tagIds.Distinct())
                {
                    var tagExists = await _tagRepository.ExistsAsync(t => t.TagID == tagId, cancellationToken);
                    if (tagExists)
                    {
                        await _context.NewsTags.AddAsync(new NewsTag
                        {
                            NewsArticleID = existing.NewsArticleID,
                            TagID = tagId
                        }, cancellationToken);
                    }
                }
            }

            _newsRepository.Update(existing);
            await _newsRepository.SaveChangesAsync(cancellationToken);

            return (true, "News article updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(string id, CancellationToken cancellationToken = default)
        {
            var news = await _newsRepository.GetByIdAsync(id, cancellationToken);
            if (news == null)
            {
                return (false, "News article not found.");
            }

            // Remove related NewsTags first (Cascade cũng được, nhưng rõ ràng hơn)
            var tags = _context.NewsTags.Where(nt => nt.NewsArticleID == id);
            _context.NewsTags.RemoveRange(tags);

            _newsRepository.Delete(news);
            await _newsRepository.SaveChangesAsync(cancellationToken);

            return (true, "News article deleted successfully.");
        }
    }
}