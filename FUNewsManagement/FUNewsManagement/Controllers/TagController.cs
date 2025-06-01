using FUNewsManagement.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FUNewsManagement.Controllers
{
    public class TagController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public TagController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
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
