namespace FUNewsManagementSystem.API.DTOs.Categories
{
    public class CategoryResponseDto
    {
        public short CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDesciption { get; set; } = string.Empty;
        public short? ParentCategoryID { get; set; }
        public bool? IsActive { get; set; }
    }
}