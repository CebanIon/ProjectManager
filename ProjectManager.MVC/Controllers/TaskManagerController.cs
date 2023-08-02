using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using ProjectManager.Application.Projects.Queries.AddUserToProject;
using ProjectManager.Application.Projects.Queries.CreateProject;
using ProjectManager.Application.Projects.Queries.GetAllProjectsByUserId;
using ProjectManager.Application.Projects.Queries.GetAllProjectsOfUser;
using ProjectManager.Application.Projects.Queries.GetProjectById;
using ProjectManager.Application.Projects.Queries.ModifyProject;
using ProjectManager.Application.Projects.Queries.RemoveUserFromProject;
using ProjectManager.Application.ProjectState.Queries;
using ProjectManager.Application.ProjectTasks.Queries.AddUserToTask;
using ProjectManager.Application.ProjectTasks.Queries.CreateTasks;
using ProjectManager.Application.ProjectTasks.Queries.DeleteTaskById;
using ProjectManager.Application.ProjectTasks.Queries.GetAllTasksByProjectId;
using ProjectManager.Application.ProjectTasks.Queries.GetTaskById;
using ProjectManager.Application.ProjectTasks.Queries.ModifyTask;
using ProjectManager.Application.ProjectTasks.Queries.RemoveUserFromTask;
using ProjectManager.Application.TaskPriority.Queries.GetAllTaskPriorities;
using ProjectManager.Application.TaskType.Queries.GetAllTaskTypes;
using ProjectManager.Application.Users.Queries.GetUsersNotInProject;
using ProjectManager.Application.Users.Queries.GetUsersNotInTask;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

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

            if (projectId != 0)
            {
                ProjectDetailsVM selectedProject = await Mediator.Send(new GetProjectByIdQuery { Id = projectId});
                ViewBag.SelectedProject = selectedProject;
                List<UsersNotInVM> usersNoIt = await Mediator.Send(new GetusersNotInTaskQuery { ProjectId= projectId });
                ViewBag.UserNotIn = usersNoIt;
                List<ProjectStateVM> projectStates = await Mediator.Send(new GetAllProjectStateQuery());
                ViewBag.ProjectStates = projectStates;
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModifyProjectPost(int projectId, [FromForm] ModifyProjectQuery modifyProject)
        {
            modifyProject.Id = projectId;

            int result = await Mediator.Send(modifyProject);

            return RedirectToAction("Index", new { projectId = projectId });
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

            List<TaskTypeVM> taskTypes = await Mediator.Send(new GetAllTaskTypesQuery());
            ViewBag.TaskTypes = taskTypes;
            List<PriorityVM> priorities = await Mediator.Send(new GetAllTaskPrioritiesQuery());
            ViewBag.Priorities = priorities;

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
            ViewBag.TaskId = taskId;
            List<ProjectVM> projects = await Mediator.Send(new GetAllProjetsByUserIdQuery { UserId = 1 });

            ViewBag.Projects = projects;

            ProjectTaskVM projectTask = await Mediator.Send(new GetTaskByIdQuery { ProjectTaskId = taskId });

            ViewBag.ProjectTask = projectTask;
            ViewBag.ProjectTaskId = taskId;

            ViewBag.ProjectTaskId = taskId;

            List<UsersNotInVM> usersNoIt = await Mediator.Send(new GetUsersNotInProjectQuery { ProjectTaskId = taskId });
            ViewBag.UserNotIn = usersNoIt;

            List<TaskTypeVM> taskTypes = await Mediator.Send(new GetAllTaskTypesQuery());
            ViewBag.TaskTypes = taskTypes;
            List<PriorityVM> priorities = await Mediator.Send(new GetAllTaskPrioritiesQuery());
            ViewBag.Priorities = priorities;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTaskPost(int taskId ,[FromForm]ModifyTaskQuery modifyTaskQuery)
        {
            modifyTaskQuery.ModifiedBy = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            modifyTaskQuery.Id = taskId;

            await Mediator.Send(modifyTaskQuery);

            return RedirectToAction("EditTask", new { taskId  = taskId });
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
        public async Task<IActionResult> RemoveUserFromTask(int userId, int taskId)
        {
            int result = await Mediator.Send(new RemoveUserFromTaskQuery { UserId = userId, TaskId = taskId});

            return result > 0 ? Ok() : BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserFromProject(int userId, int projectId)
        {
            int result = await Mediator.Send(new RemoveUserFromProjectQuery { UserId = userId, ProjectId = projectId });

            return result > 0 ? Ok() : BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> AddUserToProject(int projectId)
        {
            if (Request.Form != default && Request.Form.Keys != default)
            {
                foreach (string key in Request.Form.Keys)
                {
                    int userId = 0;
                    if (int.TryParse(key, out userId))
                    {
                        AddUserToProjectQuery addUserToProjectQuery = new AddUserToProjectQuery
                        {
                            ProjectId = projectId,
                            UserId = userId

                        };

                        await Mediator.Send(addUserToProjectQuery);
                    }
                }
                return RedirectToAction("Index", new { projectId = projectId });
            }
            else
            {
                return BadRequest();
            }
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
                        AddUserToTaskQuery addProjectToTaskQuery = new AddUserToTaskQuery
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
