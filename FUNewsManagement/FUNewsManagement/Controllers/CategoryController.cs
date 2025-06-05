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

    [HttpGet]
    public async Task<IActionResult> GetEditModal(short id)
    {
        try
        {
            var category = await _categoryService.GetByIdAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            var model = new CategoryRequest
            {
                CategoryId = category.CategoryId,
                CategoryName = category.CategoryName,
                CategoryDescription = category.CategoryDescription,
                ParentCategoryId = category.ParentCategoryId,
                IsActive = category.IsActive
            };

            return PartialView("EditCategoryModal", model);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Failed to load category data");
        }
    }

    // POST: Category/Edit
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(CategoryRequest request)
    {
        try
        {
            if (ModelState.IsValid)
            {
                await _categoryService.UpdateAsync(request.CategoryId, request);


                // Nếu là AJAX request
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Category updated successfully!" });
                }

                // Nếu là form submit thông thường
                TempData["SuccessMessage"] = "Category updated successfully!";
                return RedirectToAction(nameof(Index));


                ModelState.AddModelError("", "Failed to update category");
            }

            // Nếu là AJAX request với lỗi
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("EditCategoryModal", request);
            }

            // Nếu là form submit thông thường với lỗi
            return View(request);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"An error occurred: {ex.Message}");

            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return PartialView("EditCategoryModal", request);
            }

            return View(request);
        }
    }

    // Thêm action Delete để xử lý yêu cầu xóa category
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(short id)
    {
        try
        {
            // Kiểm tra nếu category có tồn tại
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Category not found" });
                }

                TempData["ErrorMessage"] = "Category not found";
                return RedirectToAction(nameof(Index));
            }

            // Gọi service để xóa category
            var result = await _categoryService.DeleteAsync(id);

            // Xử lý kết quả
            if (result)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new
                        { success = true, message = $"Category '{category.CategoryName}' deleted successfully" });
                }

                TempData["SuccessMessage"] = $"Category '{category.CategoryName}' deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new
                    {
                        success = false,
                        message = "Cannot delete category. It may contain articles or other linked content."
                    });
                }

                TempData["ErrorMessage"] = "Cannot delete category. It may contain articles or other linked content.";
                return RedirectToAction(nameof(Index));
            }
        }
        catch (Exception ex)
        {
            if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                return Json(new { success = false, message = $"An error occurred: {ex.Message}" });
            }

            TempData["ErrorMessage"] = $"Failed to delete category: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }
}