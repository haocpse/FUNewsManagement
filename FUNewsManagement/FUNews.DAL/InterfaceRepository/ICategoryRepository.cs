using FUNews.DAL.Entity;

namespace FUNews.DAL.InterfaceRepository;

public interface ICategoryRepository : IBaseRepository<Category, short>
{
    // Basic CRUD operations
    Task DeleteAsync(Category category);
        
    // Extended operations for Categories
    Task<Category?> GetByIdWithDetailsAsync(short id);
    Task<bool> HasSubcategoriesAsync(short id);
    Task<IEnumerable<Category>> GetSubcategoriesAsync(short parentId);
    Task<IEnumerable<Category>> GetRootCategoriesAsync();
    Task<bool> ExistsByIdAsync(short id);
    Task<bool> IsCategoryNameUniqueAsync(string name, short? excludeId = null);
}