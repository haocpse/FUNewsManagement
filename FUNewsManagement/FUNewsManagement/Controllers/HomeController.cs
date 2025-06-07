using FuNews.Modals.DTOs.Request.Paging;
using FuNews.Modals.DTOs.Response.News;
using FuNews.Modals.DTOs.Response.Paging;
using FUNews.BLL.InterfaceService;
using FUNews.BLL.Service;
using FUNewsManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;

namespace FUNewsManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INewsService _newsService;
        private readonly ICategoryService _categoryService;

        public HomeController(ILogger<HomeController> logger, INewsService newsService, ICategoryService categoryService)
        {
            _logger = logger;
            _newsService = newsService;
            _categoryService = categoryService;
        }
        public async Task<IActionResult> Index(short? categoryId, int pageNumber = 1, int pageSize = 5)
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories;

            var pagingRequest = new PagingRequest
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            PageResult<NewsResponse> pagedNews;

            if (categoryId.HasValue)
            {
                pagedNews = await _newsService.GetNewsByCategoryAsync(categoryId, pagingRequest);
            } else
            {
                pagedNews = await _newsService.OverriedGetAllAsync(pagingRequest);
            }

            return View(pagedNews);
        }


        [HttpGet("/News/Details/{id}")]
        public async Task<IActionResult> Details(string id)
        {
            var news = await _newsService.GetById(id);
            if (news == null)
                return NotFound();

            return View(news); // s? tìm Views/News/Details.cshtml
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
