using FUNewsManagementSystem.Web.Services.Interfaces;
using FUNewsManagementSystem.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.Web.Controllers
{
    public class ReportsController : Controller
    {
        private readonly IApiService _apiService;

        public ReportsController(IApiService apiService)
        {
            _apiService = apiService;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("UserRole") == "Admin";
        }

        [HttpGet]
        public IActionResult Index()
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            return View(new ReportViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ReportViewModel model)
        {
            if (!IsAdmin())
                return RedirectToAction("Login", "Account");

            if (!ModelState.IsValid)
                return View(model);

            if (model.StartDate > model.EndDate)
            {
                model.ErrorMessage = "Start Date must be less than or equal to End Date.";
                return View(model);
            }

            model.Results = await _apiService.GetReportAsync(model.StartDate, model.EndDate);
            return View(model);
        }
    }
}