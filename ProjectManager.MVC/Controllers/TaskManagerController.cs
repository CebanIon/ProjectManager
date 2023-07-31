using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Projects.Queries.CreateProject;
using ProjectManager.Application.Projects.Queries.GetAllProjectsByUserId;
using ProjectManager.Application.Projects.Queries.GetAllProjectsOfUser;
using ProjectManager.Application.ProjectTasks.Queries.AddUserToTask;
using ProjectManager.Application.ProjectTasks.Queries.CreateTasks;
using ProjectManager.Application.ProjectTasks.Queries.DeleteTaskById;
using ProjectManager.Application.ProjectTasks.Queries.GetAllTasksByProjectId;
using ProjectManager.Application.ProjectTasks.Queries.GetTaskById;
using ProjectManager.Application.ProjectTasks.Queries.ModifyTask;
using ProjectManager.Application.Users.Queries.GetUsersNotInTask;
using System.Security.Claims;
using System.Text.Json;

namespace ProjectManager.MVC.Controllers
{
    public class TaskManagerController : BaseController
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index(int projectId = 0)
        {
            List<ProjectVM> projects = await Mediator.Send(new GetAllProjetsByUserIdQuery { UserId = 1 });

            ViewBag.Projects = projects;
            ViewBag.ProjectId = projectId;

            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreateProject()
        {
            List<ProjectVM> projects = await Mediator.Send(new GetAllProjetsByUserIdQuery { UserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) });

            ViewBag.Projects = projects;

            return View();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreateTask(int projectId = 0)
        {
            List<ProjectVM> projects = await Mediator.Send(new GetAllProjetsByUserIdQuery { UserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) });

            ViewBag.Projects = projects;
            ViewBag.ProjectId = projectId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTaskPost(CreateTaskQuery query)
        {
            query.CreatorId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            await Mediator.Send(query);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProjectPost(CreateProjectQuery query)
        {
            query.CreatorId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            await Mediator.Send(query);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditTask(int taskId)
        {
            List<ProjectVM> projects = await Mediator.Send(new GetAllProjetsByUserIdQuery { UserId = 1 });

            ViewBag.Projects = projects;

            ProjectTaskVM projectTask = await Mediator.Send(new GetTaskByIdQuery { ProjectTaskId = taskId });

            ViewBag.ProjectTask = projectTask;
            ViewBag.ProjectTaskId = taskId;

            ViewBag.ProjectTaskId = taskId;

            List<UsersNotInVM> usersNoIt = await Mediator.Send(new GetusersNotInTaskQuery { ProjectTaskId = taskId });
            ViewBag.UserNotIn = usersNoIt;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTaskPost(int taskId ,[FromForm]ModifyTaskQuery modifyTaskQuery)
        {
            modifyTaskQuery.ModifiedBy = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            modifyTaskQuery.Id = taskId;

            await Mediator.Send(modifyTaskQuery);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> ReturnData(int projectId = 0)
        {
            List<ProjectTaskRowVM> projectTasks = await Mediator.Send(new GetAllTasksByProjectIdQuery { ProjectId = projectId });

            return Ok(projectTasks.ToArray());
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            await Mediator.Send(new DeleteTaskByIdQuery { TaskId = taskId });

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToTask(int taskId)
        {
            if (Request.Form != default && Request.Form.Keys != default)
            {
                foreach (string key in Request.Form.Keys)
                {
                    int userId = 0;
                    if (int.TryParse(key, out userId))
                    {
                        AddProjectToTaskQuery addProjectToTaskQuery = new AddProjectToTaskQuery
                        {
                            TaskId = taskId,
                            UserId = userId

                        };

                        await Mediator.Send(addProjectToTaskQuery);
                    }
                }
                return RedirectToAction("EditTask", new { taskId = taskId});
            }
            else 
            {
                return BadRequest();
            }
        }
    }
}
