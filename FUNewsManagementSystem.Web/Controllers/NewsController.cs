using FUNewsManagementSystem.Web.Services.Interfaces;
using FUNewsManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.Web.Controllers
{
    public class NewsController : Controller
    {
        private readonly IApiService _apiService;

        public NewsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        /// <summary>
        /// Public News List + Search (chỉ Active News)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(string? keyword, int page = 1)
        {
            const int pageSize = 6;

            var allNews = await _apiService.GetActiveNewsAsync(keyword);

            var totalItems = allNews.Count;
            var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            if (page < 1) page = 1;
            if (page > totalPages && totalPages > 0) page = totalPages;

            var pagedNews = allNews
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var viewModel = new NewsListViewModel
            {
                NewsList = pagedNews,
                Keyword = keyword,
                CurrentPage = page,
                TotalPages = totalPages,
                TotalItems = totalItems
            };

            return View(viewModel);
        }

        /// <summary>
        /// Public News Detail (chỉ hiện nếu Active)
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return NotFound();

            var news = await _apiService.GetNewsByIdAsync(id);

            // Guest chỉ được xem tin Active
            if (news == null || news.NewsStatus != true)
                return NotFound();

            return View(news);
        }
    }
}