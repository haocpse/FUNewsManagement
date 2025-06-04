using FUNews.BLL.InterfaceService;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using FuNews.Modals.DTOs.Response.Category;
using FUNewsManagement.Models.Request;
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
        
        [HttpGet]
        public IActionResult OpenCreateModal(short? parentId)
        {
            // Khởi tạo model với parentId được truyền vào (nếu có)
            var model = new CategoryRequest { ParentCategoryId = parentId };
    
            // Có thể thêm logic để hiển thị tên category cha nếu cần
            if (parentId.HasValue)
            {
                // Optional: lấy thông tin category cha để hiển thị
                var parentCategory = _categoryService.GetByIdAsync(parentId.Value).Result;
                if (parentCategory != null)
                {
                    ViewBag.ParentCategoryName = parentCategory.CategoryName;
                }
            }
    
            return PartialView("CreateCategoryModal", model);
        }
        
        
        [HttpPost]
        public async Task<IActionResult> Create(CategoryRequest request)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Call service to create category
                    var result = await _categoryService.CreateAsync(request);
            
                    if (request.ParentCategoryId.HasValue)
                    {
                        // If this is an AJAX request and created a subcategory
                        if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                        {
                            return Json(new { success = true, message = "Category created successfully" });
                        }
                    }
            
                    // Normal request or root category
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error creating category: {ex.Message}");
                }
            }
    
            // If it's an AJAX request
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return BadRequest(ModelState);
            }
    
            // Return to form with errors
            return View(request);
        }
        
    }