using FUNews.DAL.InterfaceRepository;
using Microsoft.EntityFrameworkCore;
using FUNews.DAL.Entity;

namespace FUNews.DAL.Repository
{
    public class CategoryRepository(FUNewsDbContext context)
        : BaseRepository<Category, short>(context), ICategoryRepository
    {
        public async Task<Category?> GetByIdWithDetailsAsync(short id)
        {
            return await _context.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task DeleteAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> HasSubcategoriesAsync(short id)
        {
            return await _context.Categories
                .AnyAsync(c => c.ParentCategoryId == id);
        }

        public async Task<IEnumerable<Category>> GetSubcategoriesAsync(short parentId)
        {
            return await _context.Categories
                .Where(c => c.ParentCategoryId == parentId)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<Category>> GetRootCategoriesAsync()
        {
            return await _context.Categories
                .Where(c => c.ParentCategoryId == null)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<bool> ExistsByIdAsync(short id)
        {
            return await _context.Categories
                .AnyAsync(c => c.CategoryId == id);
        }

        public async Task<bool> IsCategoryNameUniqueAsync(string name, short? excludeId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be null or empty", nameof(name));

            var query = _context.Categories.Where(c => c.CategoryName == name);

            if (excludeId.HasValue)
            {
                query = query.Where(c => c.CategoryId != excludeId);
            }

            return !await query.AnyAsync();
        }
    }
}