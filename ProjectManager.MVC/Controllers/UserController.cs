using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.MVC.Controllers
{
    public class UserController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
