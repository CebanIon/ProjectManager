using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ProjectManager.MVC.Controllers
{
    [Authorize(Roles ="Administrator")]
    public class UserController : BaseController
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View("_Index");
        }
    }
}
