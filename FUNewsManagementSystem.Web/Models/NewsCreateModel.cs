namespace FUNewsManagementSystem.Web.Models
{
    public class NewsCreateModel
    {
        public string NewsArticleID { get; set; } = string.Empty;
        public string? NewsTitle { get; set; }
        public string Headline { get; set; } = string.Empty;
        public string? NewsContent { get; set; }
        public string? NewsSource { get; set; }
        public short CategoryID { get; set; }
        public bool? NewsStatus { get; set; } = true;
        public List<int>? TagIds { get; set; }
    }
}