using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.MVC.Controllers
{
    public class UserController : BaseController
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View("_Index");
        }
    }
}
