using FUNews.BLL.InterfaceService;
using FUNewsManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FUNewsManagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INewsService _newsService;

        public HomeController(ILogger<HomeController> logger, INewsService newsService)
        {
            _logger = logger;
            _newsService = newsService;
        }
        public async Task<IActionResult> Index()
        {
            var list = await _newsService.OverriedGetAllAsync();
            return View(list);
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
