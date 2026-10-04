using Microsoft.AspNetCore.Mvc;

namespace FUNewsManagementSystem.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return RedirectToAction("Index", "News");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}