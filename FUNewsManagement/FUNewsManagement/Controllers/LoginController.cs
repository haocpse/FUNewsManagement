using FuNews.Modals.DTOs.Request;
using FUNews.BLL.InterfaceService;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagement.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILogger<SystemAccountController> _logger;
        private readonly ISystemAccountService _systemAccountService;

        public LoginController(ILogger<SystemAccountController> logger, ISystemAccountService systemAccountService)
        {
            _logger = logger;
            _systemAccountService = systemAccountService;
        }
        public IActionResult Index()
        {
            return View();
        }

        // POST: /SystemAccount/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _systemAccountService.login(request);
            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Sai email hoặc mật khẩu.");
                return View(request);
            }

            TempData["LoginSuccess"] = $"Xin chào {result.AccountName}";
            HttpContext.Session.SetString("AccountEmail", result.AccountEmail);
            return RedirectToAction("Profile", new { email = result.AccountEmail });
        }
    }
}
