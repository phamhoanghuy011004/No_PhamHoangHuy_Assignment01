using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FUNewsManagementSystem.API.Models.Entities
{
    [Table("SystemAccount")]
    public class SystemAccount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public short AccountID { get; set; }

        [StringLength(100)]
        public string? AccountName { get; set; }

        [StringLength(70)]
        public string? AccountEmail { get; set; }

        public int? AccountRole { get; set; }

        [StringLength(70)]
        public string? AccountPassword { get; set; }

        // Navigation property
        public virtual ICollection<NewsArticle> CreatedNewsArticles { get; set; } = new List<NewsArticle>();
    }
}