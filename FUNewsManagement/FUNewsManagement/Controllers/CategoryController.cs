using FUNews.BLL.InterfaceService;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using FuNews.Modals.DTOs.Response.Category;
using CategoryResponse = FUNews.Modals.DTOs.Response.CategoryResponse;

namespace FUNewsManagement.Controllers;

public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            // Get all categories
            var allCategories = await _categoryService.GetAllAsync();
            // Convert to tree structure
            var categoryTree = _categoryService.BuildCategoryTree(allCategories);
            return View(categoryTree);
        }

        
    }