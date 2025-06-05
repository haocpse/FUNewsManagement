using System.ComponentModel.DataAnnotations;

namespace FUNews.Modals.DTOs.Response
{
    public class CategoryResponse
    {
        public short CategoryId { get; set; }
        
        public string CategoryName { get; set; } = string.Empty;
        
        public string CategoryDescription { get; set; } = string.Empty;
        
        public short? ParentCategoryId { get; set; }
        
        public bool IsActive { get; set; }
        
        public string ParentCategoryName { get; set; } = string.Empty;
        
        public List<CategoryResponse> SubCategories { get; set; } = new List<CategoryResponse>();
        
        // Tiện ích cho việc hiển thị trên UI
        public int SubCategoryCount => SubCategories?.Count ?? 0;
        
        public string StatusText => IsActive ? "Active" : "Inactive";
        
        public bool HasParent => ParentCategoryId.HasValue;
        
    }
}