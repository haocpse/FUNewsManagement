using System.ComponentModel.DataAnnotations;

namespace FUNewsManagement.Models.Request;

public class CategoryRequest
{
    [Required(ErrorMessage = "Category name is required")]
    [StringLength(100, ErrorMessage = "Category name cannot exceed 100 characters")]
    [Display(Name = "Category Name")]
    public string CategoryName { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    [Display(Name = "Description")]
    public string CategoryDescription { get; set; } = string.Empty;

    [Display(Name = "Parent Category")]
    public short? ParentCategoryId { get; set; }

    [Display(Name = "Active Status")]
    public bool IsActive { get; set; } = true;
}