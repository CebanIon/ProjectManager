using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.MVC.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AllProjects()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AllTasks()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AllUsers()
        {
            return View();
        }
    }
}
