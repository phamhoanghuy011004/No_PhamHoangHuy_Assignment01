using System.ComponentModel.DataAnnotations;

namespace FUNewsManagementSystem.API.DTOs.Categories
{
    public class CategoryCreateDto
    {
        [Required(ErrorMessage = "CategoryName is required")]
        [StringLength(100, ErrorMessage = "CategoryName cannot exceed 100 characters")]
        public string CategoryName { get; set; } = string.Empty;

        [Required(ErrorMessage = "CategoryDesciption is required")]
        [StringLength(250, ErrorMessage = "CategoryDesciption cannot exceed 250 characters")]
        public string CategoryDesciption { get; set; } = string.Empty;

        public short? ParentCategoryID { get; set; }

        public bool? IsActive { get; set; } = true;
    }
}