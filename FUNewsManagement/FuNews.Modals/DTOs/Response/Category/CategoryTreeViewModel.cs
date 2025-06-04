namespace FuNews.Modals.DTOs.Response.Category;

public class CategoryTreeViewModel
{
    public short CategoryId { get; set; }
    public string CategoryName { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public short? ParentCategoryId { get; set; }
    public List<CategoryTreeViewModel> Children { get; set; } = new List<CategoryTreeViewModel>();
}