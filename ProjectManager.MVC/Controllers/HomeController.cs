using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Projects.Queries.GetAllProjectsByUserId;
using ProjectManager.Application.Projects.Queries.GetAllProjectsOfUser;
using ProjectManager.Application.ProjectTasks.Queries.GetInProgressTasksByUserId;
using ProjectManager.Application.ProjectTasks.Queries.GetPendingTasksByUserId;
using ProjectManager.Application.ProjectTasks.Queries.GetUpcomingTasksDueByUserId;
using ProjectManager.MVC.Models;
using System.Collections.Generic;
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
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            IList<InProgressTaskVM> tasksInProgress = await Mediator.Send(new GetInProgressTasksByUserIdQuery { UserId = userId });
            ViewBag.TasksInProgress = tasksInProgress;

            IList<PendingTasksVM> tasksPending = await Mediator.Send(new GetPendingTasksByUserIdQuery { UserId = userId });
            ViewBag.TasksPending = tasksPending;

            List<ProjectVM> allProjects = await Mediator.Send(new GetAllProjetsByUserIdQuery { UserId = userId });
            ViewBag.AllProjects = allProjects;

            IList<UpcommingTasksVM> tasksUpcomming = await Mediator.Send(new GetUpcomingTasksDueByUserIdQuery { UserId = userId});
            ViewBag.TasksUpcomming = tasksUpcomming;

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