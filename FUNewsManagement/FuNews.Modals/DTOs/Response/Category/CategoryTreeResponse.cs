namespace FuNews.Modals.DTOs.Response.Category;

public class CategoryTreeResponse
{
    public short CategoryId { get; set; }
        
    public string CategoryName { get; set; } = string.Empty;
        
    public bool IsActive { get; set; }
        
    public List<CategoryTreeResponse> Children { get; set; } = new List<CategoryTreeResponse>();
        
    public int Level { get; set; }
        
    public string IndentedName => new string('-', Level * 2) + " " + CategoryName;
}