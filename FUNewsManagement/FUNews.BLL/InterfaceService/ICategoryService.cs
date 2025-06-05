using FUNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response.Category;
using FUNewsManagement.Models.Request;
using CategoryResponse = FUNews.Modals.DTOs.Response.CategoryResponse;

namespace FUNews.BLL.InterfaceService;

public interface ICategoryService 
{
    // Lấy thông tin một category theo ID
    Task<CategoryResponse> GetByIdAsync(short id);
        
    // Lấy danh sách tất cả category
    Task<IEnumerable<CategoryResponse>> GetAllAsync();
        
    // Tạo mới một category
    Task<CategoryResponse> CreateAsync(CategoryRequest categoryRequest);
        
    // Cập nhật một category
    Task<CategoryResponse> UpdateAsync(short id, CategoryRequest categoryRequest);
        
    // Xóa một category
    Task<bool> DeleteAsync(short id);

    List<CategoryTreeViewModel> BuildCategoryTree(IEnumerable<CategoryResponse> categories);
}
