using AutoMapper;
using FUNews.BLL.InterfaceService;
using FUNews.DAL.Entity;
using FUNews.DAL.InterfaceRepository;
using FuNews.Modals.DTOs.Response.Category;
using FUNewsManagement.Models.Request;
using CategoryResponse = FUNews.Modals.DTOs.Response.CategoryResponse;

namespace FUNews.BLL.Service;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<CategoryResponse> CreateAsync(CategoryRequest request)
    {
        // Map request to entity
        var category = _mapper.Map<Category>(request);

        category.IsActive = request.IsActive;
        
        // Add to repository
        await _categoryRepository.AddAsync(category);
        
        // Map entity to response and return
        return _mapper.Map<CategoryResponse>(category);
    }

    public async Task<bool> DeleteAsync(short id)
    {
        // Get the entity
        var category = await _categoryRepository.GetByIdAsync(id);
        
        // Check if entity exists
        if (category == null)
        {
            return false;
        }

        // Check if the category has subcategories
        var hasSubcategories = await _categoryRepository.HasSubcategoriesAsync(id);
        if (hasSubcategories)
        {
            // Cannot delete category with subcategories
            return false;
        }

        // Delete the entity
        await _categoryRepository.DeleteAsync(category);
        return true;
    }

    public async Task<IEnumerable<CategoryResponse>> GetAllAsync()
    {
        // Get all categories
        var categories = await _categoryRepository.GetAllAsync();
        
        // Map to response objects
        return _mapper.Map<IEnumerable<CategoryResponse>>(categories);
    }

    public async Task<CategoryResponse> GetByIdAsync(short id)
    {
        // Get category with navigation properties
        var category = await _categoryRepository.GetByIdWithDetailsAsync(id);
        
        // Return null if not found
        if (category == null)
        {
            return null;
        }
        
        // Map to response
        CategoryResponse categoryResponse = _mapper.Map<CategoryResponse>(category);
        
        // Set parent category name if exists
        if (category.ParentCategory != null)
        {
            categoryResponse.ParentCategoryName = category.ParentCategory.CategoryName;
        }
        
        return categoryResponse;
    }

    public async Task<CategoryResponse> UpdateAsync(short id, CategoryRequest request)
    {
        // Get existing category
        var category = await _categoryRepository.GetByIdAsync(id);
        
        // Return null if not found
        if (category == null)
        {
            return null;
        }
        
        // Update properties
        category.CategoryName = request.CategoryName;
        category.CategoryDescription = request.CategoryDescription;
        category.ParentCategoryId = request.ParentCategoryId;
        
        // Only update IsActive if provided
        category.IsActive = request.IsActive;
        
        // Update in repository
        await _categoryRepository.UpdateAsync(category);
        
        // Return updated entity as response
        return _mapper.Map<CategoryResponse>(category);
    }
    
    public List<CategoryTreeViewModel> BuildCategoryTree(IEnumerable<CategoryResponse> categories)
    {
        // First, create a lookup dictionary for quick access
        Dictionary<short, CategoryTreeViewModel> lookup = new Dictionary<short, CategoryTreeViewModel>();
            
        // Convert all categories to view models
        foreach (var category in categories)
        {
            lookup[category.CategoryId] = new CategoryTreeViewModel
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                Description = category.CategoryDescription,
                IsActive = category.IsActive,
                ParentCategoryId = category.ParentCategoryId,
                Children = new List<CategoryTreeViewModel>()
            };
        }
            
        // Build the tree structure
        List<CategoryTreeViewModel> rootCategories = new List<CategoryTreeViewModel>();
            
        foreach (var item in lookup.Values)
        {
            // If it's a root category (no parent), add it to our root list
            if (item.ParentCategoryId == null)
            {
                rootCategories.Add(item);
            }
            else if (lookup.ContainsKey(item.ParentCategoryId.Value))
            {
                // Otherwise, add it as a child to its parent
                lookup[item.ParentCategoryId.Value].Children.Add(item);
            }
            else
            {
                // If parent doesn't exist, treat it as root
                rootCategories.Add(item);
            }
        }
            
        return rootCategories;
    }
}