using System.ComponentModel.DataAnnotations;

namespace FUNews.Modals.DTOs.Response
{
    public class CategoryResponse
    {
        public short CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public string CategoryDescription { get; set; } = string.Empty;

        public short? ParentCategoryId { get; set; }

        public bool? IsActive { get; set; }
    }
}