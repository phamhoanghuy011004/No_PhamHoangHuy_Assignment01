namespace FUNewsManagementSystem.Web.Models
{
    public class NewsUpdateModel
    {
        public string? NewsTitle { get; set; }
        public string Headline { get; set; } = string.Empty;
        public string? NewsContent { get; set; }
        public string? NewsSource { get; set; }
        public short CategoryID { get; set; }
        public bool? NewsStatus { get; set; }
        public List<int>? TagIds { get; set; }
    }
}