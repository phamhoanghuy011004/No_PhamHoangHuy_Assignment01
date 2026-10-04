using FUNewsManagementSystem.API.Models.Entities;
using FUNewsManagementSystem.API.Repositories.Interfaces;
using FUNewsManagementSystem.API.Services.Interfaces;

namespace FUNewsManagementSystem.API.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;

        public TagService(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IEnumerable<Tag>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _tagRepository.GetAllAsync(cancellationToken);
        }

        public async Task<Tag?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _tagRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Tag>> SearchAsync(string? keyword, CancellationToken cancellationToken = default)
        {
            return await _tagRepository.SearchAsync(keyword, cancellationToken);
        }
    }
}