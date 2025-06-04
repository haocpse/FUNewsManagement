namespace FuNews.Modals.DTOs.Response.Category;

public class CategoryListItemResponse
{
    public short CategoryId { get; set; }
        
    public string CategoryName { get; set; } = string.Empty;
        
    public string CategoryDescription { get; set; } = string.Empty;
        
    public short? ParentCategoryId { get; set; }
        
    public string ParentCategoryName { get; set; } = string.Empty;
        
    public bool IsActive { get; set; }
        
    public int SubCategoryCount { get; set; }
}