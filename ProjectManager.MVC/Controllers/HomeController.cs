using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.ProjectTasks.Queries.GetInProgressTasksByUserId;
using ProjectManager.MVC.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace ProjectManager.MVC.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            IList<InProgressTaskVM> tasksInProgress = await Mediator.Send(new GetInProgressTasksByUserIdQuery { UserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) });

            ViewBag.TasksInProgress = tasksInProgress;

            return View("Index");
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