using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.API.DTOs.News
{
    public class NewsCreateDto
    {
        [Required(ErrorMessage = "NewsArticleID is required")]
        [StringLength(20, ErrorMessage = "NewsArticleID cannot exceed 20 characters")]
        public string NewsArticleID { get; set; } = string.Empty;

        [StringLength(400, ErrorMessage = "NewsTitle cannot exceed 400 characters")]
        public string? NewsTitle { get; set; }

        [Required(ErrorMessage = "Headline is required")]
        [StringLength(150, ErrorMessage = "Headline cannot exceed 150 characters")]
        public string Headline { get; set; } = string.Empty;

        [StringLength(4000, ErrorMessage = "NewsContent cannot exceed 4000 characters")]
        public string? NewsContent { get; set; }

        [StringLength(400, ErrorMessage = "NewsSource cannot exceed 400 characters")]
        public string? NewsSource { get; set; }

        [Required(ErrorMessage = "CategoryID is required")]
        public short CategoryID { get; set; }

        public bool? NewsStatus { get; set; } = true;

        public List<int>? TagIds { get; set; }
    }
}