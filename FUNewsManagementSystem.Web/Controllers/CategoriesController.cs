using FUNewsManagementSystem.Web.Models;
using FUNewsManagementSystem.Web.Services.Interfaces;
using FUNewsManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly IApiService _apiService;

        public CategoriesController(IApiService apiService)
        {
            _apiService = apiService;
        }

        private bool IsStaff() => HttpContext.Session.GetString("UserRole") == "Staff";

        public async Task<IActionResult> Index(string? keyword)
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            var list = await _apiService.GetCategoriesAsync(keyword);
            return View(new CategoryListViewModel { Categories = list, Keyword = keyword });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateModel model)
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            var (success, message) = await _apiService.CreateCategoryAsync(model);
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(short id, CategoryUpdateModel model)
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            var (success, message) = await _apiService.UpdateCategoryAsync(id, model);
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(short id)
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            var (success, message) = await _apiService.DeleteCategoryAsync(id);
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Index));
        }
    }
}