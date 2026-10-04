using FUNewsManagementSystem.Web.Models;
using FUNewsManagementSystem.Web.Services.Interfaces;
using FUNewsManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.Web.Controllers
{
    public class NewsManagementController : Controller
    {
        private readonly IApiService _apiService;

        public NewsManagementController(IApiService apiService)
        {
            _apiService = apiService;
        }

        private bool IsStaff() => HttpContext.Session.GetString("UserRole") == "Staff";

        public async Task<IActionResult> Index(string? keyword)
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            var news = await _apiService.GetAllNewsAsync(keyword);
            var categories = await _apiService.GetActiveCategoriesAsync();
            var tags = await _apiService.GetTagsAsync();

            return View(new NewsManagementViewModel
            {
                NewsList = news,
                Categories = categories,
                Tags = tags,
                Keyword = keyword
            });
        }

        public async Task<IActionResult> History()
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            var news = await _apiService.GetMyNewsHistoryAsync();
            return View(news);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NewsCreateModel model)
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            var (success, message) = await _apiService.CreateNewsAsync(model);
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, NewsUpdateModel model)
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            var (success, message) = await _apiService.UpdateNewsAsync(id, model);
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            if (!IsStaff()) return RedirectToAction("Login", "Account");

            var (success, message) = await _apiService.DeleteNewsAsync(id);
            TempData[success ? "Success" : "Error"] = message;
            return RedirectToAction(nameof(Index));
        }
    }
}