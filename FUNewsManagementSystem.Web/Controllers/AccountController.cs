using FUNewsManagementSystem.Web.Models;
using FUNewsManagementSystem.Web.Services.Interfaces;
using FUNewsManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly IApiService _apiService;

        public AccountController(IApiService apiService)
        {
            _apiService = apiService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var loginModel = new LoginModel
            {
                Email = model.Email,
                Password = model.Password
            };

            var result = await _apiService.LoginAsync(loginModel);

            if (result == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid email or password.");
                return View(model);
            }

            // Lưu token vào Session
            HttpContext.Session.SetString("JWToken", result.Token);
            HttpContext.Session.SetString("UserEmail", result.Email);
            HttpContext.Session.SetString("UserRole", result.Role);
            HttpContext.Session.SetString("UserName", result.AccountName ?? result.Email);

            if (result.AccountId.HasValue)
                HttpContext.Session.SetInt32("AccountId", result.AccountId.Value);

            // Redirect theo Role
            if (result.Role == "Admin")
                return RedirectToAction("Index", "Accounts"); // Tạm thời, sau sẽ đổi thành Admin Dashboard

            if (result.Role == "Staff")
                return RedirectToAction("Index", "NewsManagement"); // Tạm thời

            return RedirectToAction("Index", "News");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "News");
        }
    }
}