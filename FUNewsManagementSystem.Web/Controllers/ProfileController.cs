using FUNewsManagementSystem.Web.Models;
using FUNewsManagementSystem.Web.Services.Interfaces;
using FUNewsManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.Web.Controllers
{
    public class ProfileController : Controller
    {
        private readonly IApiService _apiService;

        public ProfileController(IApiService apiService)
        {
            _apiService = apiService;
        }

        private bool IsStaff() => HttpContext.Session.GetString("UserRole") == "Staff";

        public async Task<IActionResult> Index()
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            var profile = await _apiService.GetProfileAsync();
            if (profile == null)
            {
                TempData["Error"] = "Cannot load profile. Backend may restrict access.";
                return View(new ProfileViewModel
                {
                    AccountName = HttpContext.Session.GetString("UserName") ?? "",
                    AccountEmail = HttpContext.Session.GetString("UserEmail") ?? ""
                });
            }

            return View(new ProfileViewModel
            {
                AccountID = profile.AccountID,
                AccountName = profile.AccountName ?? "",
                AccountEmail = profile.AccountEmail ?? ""
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ProfileViewModel model)
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid) return View(model);

            var updateModel = new ProfileUpdateModel
            {
                AccountName = model.AccountName,
                AccountPassword = model.AccountPassword
            };

            var (success, message) = await _apiService.UpdateProfileAsync(updateModel);
            TempData[success ? "Success" : "Error"] = message;

            if (success)
                HttpContext.Session.SetString("UserName", model.AccountName);

            return RedirectToAction(nameof(Index));
        }
    }
}