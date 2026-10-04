using FUNewsManagementSystem.Web.Models;

namespace FUNewsManagementSystem.Web.ViewModels
{
    public class CategoryListViewModel
    {
        public List<CategoryModel> Categories { get; set; } = new();
        public string? Keyword { get; set; }
    }
}