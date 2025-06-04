using FuNews.Modals.DTOs.Response.News;
using FUNews.BLL.InterfaceService;
using FUNews.Modals.DTOs.Response;
using Microsoft.AspNetCore.Mvc;

namespace YourAppNamespace.Controllers
{
    public class NewsController : Controller
    {

        private readonly INewsService _newsService;
        
        public NewsController(INewsService newsService)
        {

            _newsService = newsService;
        }
        public IActionResult Index()
        {
            // Giả lập dữ liệu
            var newsList = new List<NewsResponse>
            {
                new NewsResponse
                {
                    NewsTitle = "Tiêu đề 1",
                    Headline = "Tóm tắt 1",
                    NewsContent = "Nội dung chi tiết 1",
                    NewsSource = "Nguồn 1",
                    CreatedDate = DateTime.Now,
                    NewsStatus = true,
                    Category = new CategoryResponse { CategoryName = "Chính trị" },
                    Tags = new List<TagResponse>
                    {
                        new TagResponse { TagName = "Bầu cử" },
                        new TagResponse { TagName = "Quốc hội" }
                    }
                },
                new NewsResponse
                {
                    NewsTitle = "Tiêu đề 2",
                    Headline = "Tóm tắt 2",
                    NewsContent = "Nội dung chi tiết 2",
                    NewsSource = "Nguồn 2",
                    CreatedDate = DateTime.Now.AddDays(-1),
                    NewsStatus = false,
                    Category = new CategoryResponse { CategoryName = "Thể thao" },
                    Tags = new List<TagResponse>
                    {
                        new TagResponse { TagName = "World Cup" }
                    }
                }
            };

            return View(newsList);
        }
    }
}
