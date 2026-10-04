using FUNewsManagementSystem.API.Models.Entities;
using FUNewsManagementSystem.API.Repositories.Interfaces;
using FUNewsManagementSystem.API.Services.Interfaces;

namespace FUNewsManagementSystem.API.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _categoryRepository.GetAllAsync(cancellationToken);
        }

        public async Task<Category?> GetByIdAsync(short id, CancellationToken cancellationToken = default)
        {
            return await _categoryRepository.GetByIdAsync(id, cancellationToken);
        }

        public async Task<IEnumerable<Category>> SearchAsync(string? keyword, CancellationToken cancellationToken = default)
        {
            return await _categoryRepository.SearchAsync(keyword, cancellationToken);
        }

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync(CancellationToken cancellationToken = default)
        {
            return await _categoryRepository.GetActiveCategoriesAsync(cancellationToken);
        }

        public async Task<(bool Success, string Message)> CreateAsync(Category category, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(category.CategoryName))
            {
                return (false, "Category name is required.");
            }

            await _categoryRepository.AddAsync(category, cancellationToken);
            await _categoryRepository.SaveChangesAsync(cancellationToken);

            return (true, "Category created successfully.");
        }

        public async Task<(bool Success, string Message)> UpdateAsync(Category category, CancellationToken cancellationToken = default)
        {
            var existing = await _categoryRepository.GetByIdAsync(category.CategoryID, cancellationToken);
            if (existing == null)
            {
                return (false, "Category not found.");
            }

            existing.CategoryName = category.CategoryName;
            existing.CategoryDesciption = category.CategoryDesciption;
            existing.ParentCategoryID = category.ParentCategoryID;
            existing.IsActive = category.IsActive;

            _categoryRepository.Update(existing);
            await _categoryRepository.SaveChangesAsync(cancellationToken);

            return (true, "Category updated successfully.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(short id, CancellationToken cancellationToken = default)
        {
            var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
            if (category == null)
            {
                return (false, "Category not found.");
            }

            // Business rule: Cannot delete if category is used by any news
            var isUsed = await _categoryRepository.IsUsedByAnyNewsAsync(id, cancellationToken);
            if (isUsed)
            {
                return (false, "Cannot delete this category because it is used by news articles.");
            }

            _categoryRepository.Delete(category);
            await _categoryRepository.SaveChangesAsync(cancellationToken);

            return (true, "Category deleted successfully.");
        }
    }
}