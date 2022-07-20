using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.MVC.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
