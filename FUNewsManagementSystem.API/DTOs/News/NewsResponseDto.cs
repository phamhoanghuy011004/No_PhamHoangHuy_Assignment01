namespace FUNewsManagementSystem.API.DTOs.News
{
    public class NewsResponseDto
    {
        public string NewsArticleID { get; set; } = string.Empty;
        public string? NewsTitle { get; set; }
        public string Headline { get; set; } = string.Empty;
        public DateTime? CreatedDate { get; set; }
        public string? NewsContent { get; set; }
        public string? NewsSource { get; set; }
        public short? CategoryID { get; set; }
        public string? CategoryName { get; set; }
        public bool? NewsStatus { get; set; }
        public short? CreatedByID { get; set; }
        public string? CreatedByName { get; set; }
        public short? UpdatedByID { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<NewsTagDto> Tags { get; set; } = new();
    }
}