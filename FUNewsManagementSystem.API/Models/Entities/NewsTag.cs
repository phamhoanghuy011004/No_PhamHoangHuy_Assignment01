using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagementSystem.API.Models.Entities
{
    [Table("NewsTag")]
    public class NewsTag
    {
        [Required]
        [StringLength(20)]
        public string NewsArticleID { get; set; } = string.Empty;

        [Required]
        public int TagID { get; set; }

        // Navigation properties
        [ForeignKey(nameof(NewsArticleID))]
        public virtual NewsArticle? NewsArticle { get; set; }

        [ForeignKey(nameof(TagID))]
        public virtual Tag? Tag { get; set; }
    }
}