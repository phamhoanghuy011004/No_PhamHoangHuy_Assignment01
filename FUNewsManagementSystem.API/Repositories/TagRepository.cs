using FUNewsManagementSystem.API.Data;
using FUNewsManagementSystem.API.Models.Entities;
using FUNewsManagementSystem.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.API.Repositories
{
    public class TagRepository : GenericRepository<Tag>, ITagRepository
    {
        public TagRepository(FUNewsManagementDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Tag>> SearchAsync(string? keyword, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return await GetAllAsync(cancellationToken);
            }

            keyword = keyword.Trim().ToLower();

            return await _dbSet
                .Where(t => t.TagName != null && t.TagName.ToLower().Contains(keyword))
                .ToListAsync(cancellationToken);
        }
    }
}