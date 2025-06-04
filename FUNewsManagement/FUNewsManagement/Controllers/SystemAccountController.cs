using System.Threading.Tasks;
using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using FUNews.BLL.InterfaceService;
using FUNews.BLL.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace FUNewsManagement.Controllers
{
    [Authorize]
    public class SystemAccountController : Controller
    {
        private readonly ILogger<SystemAccountController> _logger;
        private readonly ISystemAccountService _systemAccountService;
        private readonly IConfiguration _configuration;
        private readonly int _adminRole;

        public SystemAccountController(
            ILogger<SystemAccountController> logger, 
            ISystemAccountService systemAccountService,
            IConfiguration configuration)
        {
            _logger = logger;
            _systemAccountService = systemAccountService;
            _configuration = configuration;
            _adminRole = _configuration.GetValue<int>("Roles:AdminRole");
        }

        [HttpGet]
        public async Task<IActionResult> AccountList()
        {
            var currentRole = HttpContext.Session.GetInt32("AccountRole");
            if (currentRole != _adminRole)
            {
                return Forbid();
            }

            var accountListResponse = await _systemAccountService.GetAllAccounts();
            if (accountListResponse == null || !accountListResponse.Accounts.Any())
                return NotFound("No accounts found.");

            return View(accountListResponse);
        }

        [HttpGet]
        public async Task<IActionResult> UserProfile(short id)
        {
            var currentRole = HttpContext.Session.GetInt32("AccountRole");
            var currentUserId = HttpContext.Session.GetInt32("AccountId");

            // Only allow users to view their own profile unless they're admin
            if (currentRole != _adminRole && currentUserId != id)
            {
                return Forbid();
            }

            var account = await _systemAccountService.GetAccountById(id);
            if (account == null) return NotFound();

            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateAccountRequest request)
        {
            var currentRole = HttpContext.Session.GetInt32("AccountRole");
            var currentUserId = HttpContext.Session.GetInt32("AccountId");
            bool isAdmin = currentRole == _adminRole;

            // Only allow users to update their own profile unless they're admin
            if (!isAdmin && currentUserId != request.AccountId)
            {
                return Forbid();
            }

            try
            {
                var updatedAccount = await _systemAccountService.UpdateAccount(request, isAdmin);
                if (updatedAccount == null)
                {
                    ModelState.AddModelError("", "Update failed");
                    return View(request);
                }

                TempData["SuccessMessage"] = "Update successful";
                return RedirectToAction("UserProfile", new { id = request.AccountId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(request);
            }
        }
    }
}
