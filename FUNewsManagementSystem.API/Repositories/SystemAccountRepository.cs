using FUNewsManagementSystem.API.Data;
using FUNewsManagementSystem.API.Models.Entities;
using FUNewsManagementSystem.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FUNewsManagementSystem.API.Repositories
{
    public class SystemAccountRepository : GenericRepository<SystemAccount>, ISystemAccountRepository
    {
        public SystemAccountRepository(FUNewsManagementDbContext context) : base(context)
        {
        }

        public async Task<SystemAccount?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .FirstOrDefaultAsync(a => a.AccountEmail == email, cancellationToken);
        }

        public async Task<IEnumerable<SystemAccount>> SearchAsync(string? keyword, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(keyword))
            {
                return await GetAllAsync(cancellationToken);
            }

            keyword = keyword.Trim().ToLower();

            return await _dbSet
                .Where(a =>
                    (a.AccountName != null && a.AccountName.ToLower().Contains(keyword)) ||
                    (a.AccountEmail != null && a.AccountEmail.ToLower().Contains(keyword)))
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasCreatedAnyNewsAsync(short accountId, CancellationToken cancellationToken = default)
        {
            return await _context.NewsArticles
                .AnyAsync(n => n.CreatedByID == accountId, cancellationToken);
        }
    }
}