using FUNews.DAL.Entity;

namespace FUNews.DAL.InterfaceRepository;

public interface ICategoryRepository : IBaseRepository<Category, short>
{
    // /// <summary>
    // /// Số lượng Sub‐Category (n‐child) của một category
    // /// </summary>
    // Task<bool> HasChildrenAsync(short categoryId);
    //
    // /// <summary>
    // /// Kiểm tra liệu category có bất kỳ bài NewsArticle nào không
    // /// </summary>
    // Task<bool> HasNewsAsync(short categoryId);
    //
    // /// <summary>
    // /// Trả về danh sách Category theo điều kiện tìm kiếm và phân trang
    // /// </summary>
    // Task<(List<Category> Items, int TotalCount)> GetPagedAsync(string keyword, int pageIndex, int pageSize);
}