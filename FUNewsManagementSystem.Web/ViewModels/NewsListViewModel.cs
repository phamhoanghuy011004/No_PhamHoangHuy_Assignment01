using FUNewsManagementSystem.Web.Models;

namespace FUNewsManagementSystem.Web.ViewModels
{
    public class NewsListViewModel
    {
        public List<NewsArticleModel> NewsList { get; set; } = new();
        public string? Keyword { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }
}