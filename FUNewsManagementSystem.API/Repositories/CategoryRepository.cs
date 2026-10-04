using FUNewsManagementSystem.API.Data;
using FUNewsManagementSystem.API.Models.Entities;
using FUNewsManagementSystem.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.API.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(FUNewsManagementDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> SearchAsync(string? keyword, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return await GetAllAsync(cancellationToken);
            }

            keyword = keyword.Trim().ToLower();

            return await _dbSet
                .Where(c =>
                    c.CategoryName.ToLower().Contains(keyword) ||
                    c.CategoryDesciption.ToLower().Contains(keyword))
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> IsUsedByAnyNewsAsync(short categoryId, CancellationToken cancellationToken = default)
        {
            return await _context.NewsArticles
                .AnyAsync(n => n.CategoryID == categoryId, cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(c => c.IsActive == true)
                .ToListAsync(cancellationToken);
        }
    }
}