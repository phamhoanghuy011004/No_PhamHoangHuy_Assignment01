using FUNewsManagementSystem.Web.Models;
using FUNewsManagementSystem.Web.Services.Interfaces;
using FUNewsManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.Web.Controllers
{
    public class AccountsController : Controller
    {
        private readonly IApiService _apiService;

        public AccountsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        public async Task<IActionResult> Index(string? keyword)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var accounts = await _apiService.GetAccountsAsync(keyword);
            var vm = new AccountListViewModel
            {
                Accounts = accounts,
                Keyword = keyword
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AccountCreateModel model)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid data.";
                return RedirectToAction(nameof(Index));
            }

            var (success, message) = await _apiService.CreateAccountAsync(model);
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, AccountUpdateModel model)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var (success, message) = await _apiService.UpdateAccountAsync(id, model);
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(short id)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            var (success, message) = await _apiService.DeleteAccountAsync(id);
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Index));
        }
    }
}