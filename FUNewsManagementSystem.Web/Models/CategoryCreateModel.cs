namespace FUNewsManagementSystem.Web.Models
{
    public class CategoryCreateModel
    {
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDesciption { get; set; } = string.Empty;
        public short? ParentCategoryID { get; set; }
        public bool? IsActive { get; set; } = true;
    }
}