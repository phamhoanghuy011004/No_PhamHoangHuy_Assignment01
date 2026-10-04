using FUNewsManagementSystem.Web.Models;

namespace FUNewsManagementSystem.Web.ViewModels
{
    public class NewsManagementViewModel
    {
        public List<NewsArticleModel> NewsList { get; set; } = new();
        public List<CategoryModel> Categories { get; set; } = new();
        public List<TagModel> Tags { get; set; } = new();
        public string? Keyword { get; set; }
    }
}