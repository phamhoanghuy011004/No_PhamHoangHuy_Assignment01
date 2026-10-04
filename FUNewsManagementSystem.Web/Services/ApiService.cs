using FUNewsManagementSystem.Web.Models;
using FUNewsManagementSystem.Web.Services.Interfaces;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FUNewsManagementSystem.Web.Services
{
    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        private void AddAuthorizationHeader()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");
            _httpClient.DefaultRequestHeaders.Authorization =
                !string.IsNullOrEmpty(token)
                    ? new AuthenticationHeaderValue("Bearer", token)
                    : null;
        }

        private string? ExtractMessage(string json)
        {
            try
            {
                using var doc = JsonDocument.Parse(json);
                if (doc.RootElement.TryGetProperty("message", out var msg))
                    return msg.GetString();
            }
            catch { }
            return null;
        }

        // ==================== Auth ====================
        public async Task<LoginResponseModel?> LoginAsync(LoginModel model)
        {
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/auth/login", content);
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<LoginResponseModel>(json, _jsonOptions);
        }

        // ==================== Public News ====================
        public async Task<List<NewsArticleModel>> GetActiveNewsAsync(string? keyword = null)
        {
            var response = await _httpClient.GetAsync("api/newsarticles/active");
            if (!response.IsSuccessStatusCode)
                return new List<NewsArticleModel>();

            var json = await response.Content.ReadAsStringAsync();
            var list = JsonSerializer.Deserialize<List<NewsArticleModel>>(json, _jsonOptions) ?? new();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim().ToLower();
                list = list.Where(n =>
                    (n.Headline != null && n.Headline.ToLower().Contains(keyword)) ||
                    (n.NewsTitle != null && n.NewsTitle.ToLower().Contains(keyword)) ||
                    (n.NewsContent != null && n.NewsContent.ToLower().Contains(keyword)) ||
                    (n.CategoryName != null && n.CategoryName.ToLower().Contains(keyword))
                ).ToList();
            }

            return list;
        }

        public async Task<NewsArticleModel?> GetNewsByIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"api/newsarticles/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<NewsArticleModel>(json, _jsonOptions);
        }

        // ==================== Accounts (Admin) ====================
        public async Task<List<AccountModel>> GetAccountsAsync(string? keyword = null)
        {
            AddAuthorizationHeader();
            var url = string.IsNullOrWhiteSpace(keyword) ? "api/accounts" : $"api/accounts/search?keyword={Uri.EscapeDataString(keyword)}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return new();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<AccountModel>>(json, _jsonOptions) ?? new();
        }

        public async Task<AccountModel?> GetAccountByIdAsync(short id)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.GetAsync($"api/accounts/{id}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AccountModel>(json, _jsonOptions);
        }

        public async Task<(bool Success, string Message)> CreateAccountAsync(AccountCreateModel model)
        {
            AddAuthorizationHeader();
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/accounts", content);
            var json = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode ? (true, "Account created successfully.") : (false, ExtractMessage(json) ?? "Failed to create account.");
        }

        public async Task<(bool Success, string Message)> UpdateAccountAsync(short id, AccountUpdateModel model)
        {
            AddAuthorizationHeader();
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/accounts/{id}", content);
            var json = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode ? (true, "Account updated successfully.") : (false, ExtractMessage(json) ?? "Failed to update account.");
        }

        public async Task<(bool Success, string Message)> DeleteAccountAsync(short id)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.DeleteAsync($"api/accounts/{id}");
            var json = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode ? (true, "Account deleted successfully.") : (false, ExtractMessage(json) ?? "Failed to delete account.");
        }

        // ==================== Report ====================
        public async Task<List<NewsArticleModel>> GetReportAsync(DateTime startDate, DateTime endDate)
        {
            AddAuthorizationHeader();
            var url = $"api/reports/statistic?startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return new();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<NewsArticleModel>>(json, _jsonOptions) ?? new();
        }

        // ==================== Categories (Staff) ====================
        public async Task<List<CategoryModel>> GetCategoriesAsync(string? keyword = null)
        {
            AddAuthorizationHeader();
            var url = string.IsNullOrWhiteSpace(keyword) ? "api/categories" : $"api/categories/search?keyword={Uri.EscapeDataString(keyword)}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return new();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CategoryModel>>(json, _jsonOptions) ?? new();
        }

        public async Task<List<CategoryModel>> GetActiveCategoriesAsync()
        {
            AddAuthorizationHeader();
            var response = await _httpClient.GetAsync("api/categories/active");
            if (!response.IsSuccessStatusCode) return new();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CategoryModel>>(json, _jsonOptions) ?? new();
        }

        public async Task<(bool Success, string Message)> CreateCategoryAsync(CategoryCreateModel model)
        {
            AddAuthorizationHeader();
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/categories", content);
            var json = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode ? (true, "Category created successfully.") : (false, ExtractMessage(json) ?? "Failed to create category.");
        }

        public async Task<(bool Success, string Message)> UpdateCategoryAsync(short id, CategoryUpdateModel model)
        {
            AddAuthorizationHeader();
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/categories/{id}", content);
            var json = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode ? (true, "Category updated successfully.") : (false, ExtractMessage(json) ?? "Failed to update category.");
        }

        public async Task<(bool Success, string Message)> DeleteCategoryAsync(short id)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.DeleteAsync($"api/categories/{id}");
            var json = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode ? (true, "Category deleted successfully.") : (false, ExtractMessage(json) ?? "Failed to delete category.");
        }

        // ==================== Tags ====================
        public async Task<List<TagModel>> GetTagsAsync()
        {
            AddAuthorizationHeader();
            var response = await _httpClient.GetAsync("api/tags");
            if (!response.IsSuccessStatusCode) return new();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<TagModel>>(json, _jsonOptions) ?? new();
        }

        // ==================== News Management ====================
        public async Task<List<NewsArticleModel>> GetAllNewsAsync(string? keyword = null)
        {
            AddAuthorizationHeader();
            var url = string.IsNullOrWhiteSpace(keyword) ? "api/newsarticles" : $"api/newsarticles/search?keyword={Uri.EscapeDataString(keyword)}";
            var response = await _httpClient.GetAsync(url);
            if (!response.IsSuccessStatusCode) return new();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<NewsArticleModel>>(json, _jsonOptions) ?? new();
        }

        public async Task<List<NewsArticleModel>> GetMyNewsHistoryAsync()
        {
            AddAuthorizationHeader();
            var response = await _httpClient.GetAsync("api/newsarticles/my-history");
            if (!response.IsSuccessStatusCode) return new();
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<NewsArticleModel>>(json, _jsonOptions) ?? new();
        }

        public async Task<(bool Success, string Message)> CreateNewsAsync(NewsCreateModel model)
        {
            AddAuthorizationHeader();
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/newsarticles", content);
            var json = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode ? (true, "News created successfully.") : (false, ExtractMessage(json) ?? "Failed to create news.");
        }

        public async Task<(bool Success, string Message)> UpdateNewsAsync(string id, NewsUpdateModel model)
        {
            AddAuthorizationHeader();
            var content = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/newsarticles/{id}", content);
            var json = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode ? (true, "News updated successfully.") : (false, ExtractMessage(json) ?? "Failed to update news.");
        }

        public async Task<(bool Success, string Message)> DeleteNewsAsync(string id)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.DeleteAsync($"api/newsarticles/{id}");
            var json = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode ? (true, "News deleted successfully.") : (false, ExtractMessage(json) ?? "Failed to delete news.");
        }

        // ==================== Profile ====================
        public async Task<AccountModel?> GetProfileAsync()
        {
            AddAuthorizationHeader();
            // Tạm thời dùng account id từ session (cần Backend hỗ trợ /api/profile)
            var accountId = _httpContextAccessor.HttpContext?.Session.GetInt32("AccountId");
            if (accountId == null) return null;

            var response = await _httpClient.GetAsync($"api/accounts/{accountId}");
            if (!response.IsSuccessStatusCode) return null;
            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<AccountModel>(json, _jsonOptions);
        }

        public async Task<(bool Success, string Message)> UpdateProfileAsync(ProfileUpdateModel model)
        {
            AddAuthorizationHeader();
            var accountId = _httpContextAccessor.HttpContext?.Session.GetInt32("AccountId");
            if (accountId == null) return (false, "Not logged in.");

            var updateModel = new AccountUpdateModel
            {
                AccountName = model.AccountName,
                AccountEmail = _httpContextAccessor.HttpContext?.Session.GetString("UserEmail") ?? "",
                AccountRole = 1, // Staff
                AccountPassword = model.AccountPassword
            };

            var content = new StringContent(JsonSerializer.Serialize(updateModel), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/accounts/{accountId}", content);
            var json = await response.Content.ReadAsStringAsync();
            return response.IsSuccessStatusCode ? (true, "Profile updated successfully.") : (false, ExtractMessage(json) ?? "Failed to update profile. (Backend may require Admin role)");
        }
    }
}