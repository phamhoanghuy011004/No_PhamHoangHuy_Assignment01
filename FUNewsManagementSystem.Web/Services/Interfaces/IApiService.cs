using FUNewsManagementSystem.Web.Models;

namespace FUNewsManagementSystem.Web.Services.Interfaces
{
    public interface IApiService
    {
        // Auth
        Task<LoginResponseModel?> LoginAsync(LoginModel model);

        // Public News
        Task<List<NewsArticleModel>> GetActiveNewsAsync();
        Task<NewsArticleModel?> GetNewsByIdAsync(string id);

        // Accounts (Admin)
        Task<List<AccountModel>> GetAccountsAsync(string? keyword = null);
        Task<AccountModel?> GetAccountByIdAsync(short id);
        Task<(bool Success, string Message)> CreateAccountAsync(AccountCreateModel model);
        Task<(bool Success, string Message)> UpdateAccountAsync(short id, AccountUpdateModel model);
        Task<(bool Success, string Message)> DeleteAccountAsync(short id);

        // Report (Admin)
        Task<List<NewsArticleModel>> GetReportAsync(DateTime startDate, DateTime endDate);

        // Categories (Staff)
        Task<List<CategoryModel>> GetCategoriesAsync(string? keyword = null);
        Task<List<CategoryModel>> GetActiveCategoriesAsync();
        Task<(bool Success, string Message)> CreateCategoryAsync(CategoryCreateModel model);
        Task<(bool Success, string Message)> UpdateCategoryAsync(short id, CategoryUpdateModel model);
        Task<(bool Success, string Message)> DeleteCategoryAsync(short id);

        // Tags (Staff)
        Task<List<TagModel>> GetTagsAsync();

        // News Management (Staff)
        Task<List<NewsArticleModel>> GetAllNewsAsync(string? keyword = null);
        Task<List<NewsArticleModel>> GetMyNewsHistoryAsync();
        Task<(bool Success, string Message)> CreateNewsAsync(NewsCreateModel model);
        Task<(bool Success, string Message)> UpdateNewsAsync(string id, NewsUpdateModel model);
        Task<(bool Success, string Message)> DeleteNewsAsync(string id);

        // Profile (Staff)
        Task<AccountModel?> GetProfileAsync();
        Task<(bool Success, string Message)> UpdateProfileAsync(ProfileUpdateModel model);
        Task<List<NewsArticleModel>> GetActiveNewsAsync(string? keyword = null);
    }
}