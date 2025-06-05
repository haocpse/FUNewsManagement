using FuNews.Modals.DTOs.Request._Tag;
using FuNews.Modals.DTOs.Request.News;
using FuNews.Modals.DTOs.Response.News;
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
            var AccountId = HttpContext.Session.GetInt32("AccountId");
            var Role = HttpContext.Session.GetInt32("AccountRole");
            List<NewsResponse> newsList = new();
            if (Role == 3)
            {
                newsList = await _newsService.GetAllForAdmin();
            } else
            {
                newsList = await _newsService.GetOwnedNews(short.Parse(AccountId.ToString()));
            }
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
            var tags = await _tagService.GetAllTagsAsync();
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
                var AccountId = HttpContext.Session.GetInt32("AccountId");
                await _newsService.CreateNews(short.Parse(AccountId.ToString()), request);
                return Json(new { success = true });
            }
            return PartialView("_FormCreatePartial", request);
        }

        public async Task<IActionResult> Update(string id)
        {
            var news = await _newsService.GetById(id);

            var updateRequest = new UpdateRequest
            {
                NewsArticleId = id,
                NewsTitle = news.NewsTitle,
                NewsContent = news.NewsContent,
                NewsSource = news.NewsSource,
                CategoryId = news.Category?.CategoryId,
                Headline = news.Headline,
                TagIds = news.Tags?.Select(t => t.TagId).ToList() // Gán TagId vào
            };

            ViewBag.Categories = (await _categoryService.GetAllAsync())
                .Select(c => new SelectListItem
                {
                    Value = c.CategoryId.ToString(),
                    Text = c.CategoryName
                }).ToList();

            ViewBag.Tags = (await _tagService.GetAllTagsAsync())
                .Select(t => new SelectListItem
                {
                    Value = t.TagId.ToString(),
                    Text = t.TagName
                }).ToList();
                        return PartialView("_FormUpdatePartial", updateRequest);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateRequest request)
        {
            if (ModelState.IsValid)
            {
                var AccountId = HttpContext.Session.GetInt32("AccountId");
                await _newsService.UpdateNews(short.Parse(AccountId.ToString()),  request);
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

        [HttpPost]
        public async Task<IActionResult> Approve(string id)
        {
            var success = await _newsService.ApproveNewsAsync(id);
            return Json(new { success });
        }

        [HttpGet]
        public async Task<IActionResult> Approve()
        {
            var pendingNews = await _newsService.GetNewsPendingApproval(); // trả về các bài chưa duyệt
            return View("Approve", pendingNews); // View: Views/News/Approve.cshtml
        }
    }
}
