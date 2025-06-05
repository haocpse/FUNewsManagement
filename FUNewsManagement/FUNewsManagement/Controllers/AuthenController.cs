using System.Security.Claims;
using FuNews.Modals.DTOs.Request;
using FUNews.BLL.InterfaceService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagement.Controllers
{
    public class AuthenController : Controller
    {
        private readonly ILogger<SystemAccountController> _logger;
        private readonly ISystemAccountService _systemAccountService;

        public AuthenController(ILogger<SystemAccountController> logger, ISystemAccountService systemAccountService)
        {
            _logger = logger;
            _systemAccountService = systemAccountService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Authen(LoginRequest request)
        {
            if (!ModelState.IsValid)
                return View("Index", request);

            try
            {
                var result = await _systemAccountService.login(request);
                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "Sai email hoặc mật khẩu.");
                    return View("Index", request);
                }

                // Create claims for the user
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, result.AccountName ?? string.Empty),
                    new Claim(ClaimTypes.Email, result.AccountEmail ?? string.Empty),
                    new Claim(ClaimTypes.Role, result.AccountRole?.ToString() ?? "0"),
                    new Claim("AccountId", result.AccountId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                // Store in session for backward compatibility
                HttpContext.Session.SetInt32("AccountId", result.AccountId);
                HttpContext.Session.SetInt32("AccountRole", result.AccountRole ?? 0);
                HttpContext.Session.SetString("AccountEmail", result.AccountEmail ?? string.Empty);
                HttpContext.Session.SetString("AccountName", result.AccountName ?? string.Empty); // Thêm dòng này

                TempData["LoginSuccess"] = $"Xin chào {result.AccountName}";
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Login failed for user {Email}", request.AccountEmail);
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi trong quá trình đăng nhập.");
                return View("Index", request);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // Clear authentication
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                
                // Clear session
                HttpContext.Session.Clear();
                
                // Add success message
                TempData["Message"] = "Successfully logged out";
                
                // Redirect to login page
                return RedirectToAction("Index", "Authen");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during logout");
                return RedirectToAction("Index", "Home");
            }
        }
    }
}
