using FUNews.DAL.Entity;

namespace FUNews.DAL.InterfaceRepository;

public interface ITagRepository : IBaseRepository<Tag, int>
{
    /// <summary>
    /// Trả về danh sách Tag theo điều kiện tìm kiếm và phân trang
    /// </summary>
    Task<(List<Tag> Items, int TotalCount)> GetPagedAsync(string keyword, int pageIndex, int pageSize);

    /// <summary>
    /// Kiểm tra tag có đang được gán vào bất kỳ NewsArticle nào không
    /// (Nếu bạn muốn cấm xóa tag khi đang dùng)
    /// </summary>
    Task<bool> IsUsedByAnyNewsAsync(int tagId);
}