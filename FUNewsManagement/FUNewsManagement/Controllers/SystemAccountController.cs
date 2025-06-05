using System.Threading.Tasks;
using FuNews.Modals.DTOs.Request;
using FuNews.Modals.DTOs.Response;
using FUNews.BLL.InterfaceService;
using FUNews.BLL.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

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
        public async Task<IActionResult> UserProfile(short? id = null)
        {
            var currentUserId = HttpContext.Session.GetInt32("AccountId");
            if (!currentUserId.HasValue)
            {
                return RedirectToAction("Index", "Authen");
            }

            // If no id is provided, use the current user's id
            short accountId = id ?? (short)currentUserId.Value;

            // If not admin and trying to view another user's profile, forbid
            if (accountId != currentUserId && HttpContext.Session.GetInt32("AccountRole") != _adminRole)
            {
                return Forbid();
            }

            var account = await _systemAccountService.GetAccountById(accountId);
            if (account == null) return NotFound();

            return View(account);
        }

        [HttpGet]
        public async Task<IActionResult> Update(short id)
        {
            var currentRole = HttpContext.Session.GetInt32("AccountRole");
            if (currentRole != _adminRole)
            {
                return Forbid();
            }

            var account = await _systemAccountService.GetAccountById(id);
            if (account == null) return NotFound();

            var updateRequest = new UpdateAccountRequest
            {
                AccountId = account.AccountId,
                AccountName = account.AccountName,
                AccountEmail = account.AccountEmail,
                AccountRole = account.AccountRole
            };

            // Add available roles to ViewBag for the dropdown in the view
            ViewBag.Roles = new List<SelectListItem>
            {
                new SelectListItem { Value = "1", Text = "Staff" },
                new SelectListItem { Value = "2", Text = "Lecturer" },
                new SelectListItem { Value = "3", Text = "Admin" }
            };

            return View(updateRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(UpdateAccountRequest request)
        {
            try
            {
                var currentRole = HttpContext.Session.GetInt32("AccountRole");
                if (currentRole != _adminRole)
                {
                    return Json(new { success = false, message = "Unauthorized access" });
                }

                if (!ModelState.IsValid)
                {
                    return Json(new { success = false, message = "Invalid form data" });
                }

                var updatedAccount = await _systemAccountService.UpdateAccount(request, true);
                if (updatedAccount == null)
                {
                    return Json(new { success = false, message = "Failed to update account" });
                }

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating account");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateStaffProfile()
        {
            var currentUserId = HttpContext.Session.GetInt32("AccountId");
            if (!currentUserId.HasValue)
            {
                return RedirectToAction("Index", "Authen");
            }

            var account = await _systemAccountService.GetAccountById((short)currentUserId.Value);
            if (account == null) return NotFound();

            var updateRequest = new UpdateAccountRequest
            {
                AccountId = (short)currentUserId.Value,
                AccountName = account.AccountName,
                AccountEmail = account.AccountEmail
            };

            return View("UpdateStaffProfile", updateRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStaffProfile(UpdateAccountRequest request)
        {
            var currentUserId = HttpContext.Session.GetInt32("AccountId");
            if (!currentUserId.HasValue || currentUserId.Value != request.AccountId)
            {
                return Forbid();
            }

            try
            {
                // Pass false for isAdmin to ensure staff can't modify roles
                var updatedAccount = await _systemAccountService.UpdateAccount(request, false);
                if (updatedAccount == null)
                {
                    ModelState.AddModelError("", "Update failed");
                    return View(request);
                }

                // Update session if name was changed
                if (!string.IsNullOrEmpty(updatedAccount.AccountName))
                {
                    HttpContext.Session.SetString("AccountName", updatedAccount.AccountName);
                }

                TempData["SuccessMessage"] = "Profile updated successfully";
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
