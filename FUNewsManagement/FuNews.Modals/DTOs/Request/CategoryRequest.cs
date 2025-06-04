using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement.Models.Request;

public class CategoryRequest
{
   
    public string CategoryName { get; set; } = string.Empty;

    
    public string CategoryDescription { get; set; } = string.Empty;

  
    public short? ParentCategoryId { get; set; }

   
    public bool IsActive { get; set; } = true;
}