using Microsoft.AspNetCore.Mvc;
using ShoppingWebApp.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace ShoppingWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var role = User.Claims.SingleOrDefault(c => c.Type.Equals(ClaimTypes.Role)).Value;
                if (role.Equals("Admin"))
                {
                    return RedirectToAction("Index", "Products");

                }
                else
                {
                    return RedirectToAction("Index", "Guest");
                }
            }
            else
            {
                return RedirectToAction("Index", "Guest");
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
