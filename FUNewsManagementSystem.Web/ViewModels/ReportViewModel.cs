using FUNewsManagementSystem.Web.Models;
using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.Web.ViewModels
{
    public class ReportViewModel
    {
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; } = DateTime.Today.AddMonths(-1);

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; } = DateTime.Today;

        public List<NewsArticleModel> Results { get; set; } = new();
        public string? ErrorMessage { get; set; }
    }
}