using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ShoppingWebApp.Controllers
{
    public class LogoutController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Login");
        }
    }
}