using FuNews.Modals.DTOs.Request._Tag;
using FuNews.Modals.DTOs.Request.News;
using FUNews.BLL.InterfaceService;
using FUNews.BLL.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FUNewsManagement.Controllers
{
    public class NewsController : Controller
    {

        private readonly INewsService _newsService;
        private readonly INewsTagService _newsTagService;
        private readonly ITagService _tagService;
        private readonly ICategoryService _categoryService;

        public NewsController(INewsService newsService, INewsTagService newsTagService, ITagService tagService, ICategoryService categoryService)
        {
            _newsService = newsService;
            _newsTagService = newsTagService;
            _tagService = tagService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var newsList = await _newsService.OverriedGetAllAsync(); // user id
            return View(newsList);
        }
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();
            ViewBag.Categories = categories
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList();
            var tags = await _tagService.GetAllAsync(); 
            ViewBag.Tags = tags
                .Select(t => new SelectListItem
                {
                    Value = t.TagId.ToString(),
                    Text = t.TagName
                }).ToList();
            return PartialView("_FormCreatePartial", new NewsRequest());
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewsRequest request)
        {
            if (ModelState.IsValid)
            {
                await _newsService.CreateNews(request);
                return Json(new { success = true });
            }
            return PartialView("_FormCreatePartial", request);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var news = await _newsService.GetById(id);
            var newsTag = news.Tags;
            List<NewsTagRequest> requests = new List<NewsTagRequest>();
            foreach (var tag in newsTag)
            {
                requests.Add(new()
                {
                    TagId = tag.TagId,
                    TagName = tag.TagName,
                });
            }
            var updateRequest = new UpdateRequest
            {
                NewsArticleId = id,
                NewsTitle = news.NewsTitle,
                NewsContent = news.NewsContent,
                NewsSource = news.NewsSource,
                CategoryId = news.Category?.CategoryId,
                Headline = news.Headline,
                Tags = requests
            };
            return PartialView("_FormUpdatePartial", updateRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UpdateRequest request)
        {
            if (ModelState.IsValid)
            {
                await _newsService.UpdateNews(request);
                return Json(new { success = true });
            }
            return PartialView("_FormUpdatePartial", request);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await _newsService.DeleteNews(id);
            return Json(new { success = true });
        }
    }
}
