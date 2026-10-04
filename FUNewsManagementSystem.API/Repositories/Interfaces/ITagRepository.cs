using FUNewsManagementSystem.API.Models.Entities;

namespace FUNewsManagementSystem.API.Repositories.Interfaces
{
    public interface ITagRepository : IGenericRepository<Tag>
    {
        Task<IEnumerable<Tag>> SearchAsync(string? keyword, CancellationToken cancellationToken = default);
    }
}