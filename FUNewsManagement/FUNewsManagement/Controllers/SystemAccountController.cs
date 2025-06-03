using System.Threading.Tasks;
using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using FUNews.BLL.InterfaceService;
using FUNews.BLL.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FUNewsManagement.Controllers
{
    public class SystemAccountController : Controller
    {
        private readonly ILogger<SystemAccountController> _logger;
        private readonly ISystemAccountService _systemAccountService;

        public SystemAccountController(ILogger<SystemAccountController> logger, ISystemAccountService systemAccountService)
        {
            _logger = logger;
            _systemAccountService = systemAccountService;
        }

        [HttpGet]
        public async Task<IActionResult> Update(short id)
        {
            var account = await _systemAccountService.GetAccountById(id);
            if (account == null) return NotFound();

            var request = new UpdateAccountRequest
            {
                AccountId = id,
                AccountName = account.AccountName,
                AccountEmail = account.AccountEmail,
                AccountPassword = account.AccountPassword,
                AccountRole = account.AccountRole
            };

            // Gán thông tin quyền vào ViewBag
            ViewBag.IsAdmin = HttpContext.Session.GetString("AccountRole") == "1";
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> Update(UpdateAccountRequest request)
        {
            bool isAdmin = HttpContext.Session.GetString("AccountRole") == "1";

            try
            {
                var updatedAccount = await _systemAccountService.UpdateAccount(request, isAdmin);
                if (updatedAccount == null)
                {
                    ModelState.AddModelError("", "Update fail");
                    return View(request);
                }

                TempData["SuccessMessage"] = "Update success";
                return RedirectToAction("Update", new { id = request.AccountId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(request);
            }
        }
    }
}
