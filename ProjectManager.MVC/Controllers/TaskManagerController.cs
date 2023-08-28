using FluentValidation;
using FluentValidation.Results;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using ProjectManager.Application.DTO_s.Projects;
using ProjectManager.Application.DTO_s.ProjectTasks;
using ProjectManager.Application.Projects.Commands.AddUserToProject;
using ProjectManager.Application.Projects.Commands.CreateProject;
using ProjectManager.Application.Projects.Commands.ModifyProject;
using ProjectManager.Application.Projects.Commands.RemoveUserFromProject;
using ProjectManager.Application.Projects.Queries.GetAllProjectsByUserId;
using ProjectManager.Application.Projects.Queries.GetAllProjectsOfUser;
using ProjectManager.Application.Projects.Queries.GetProjectById;
using ProjectManager.Application.ProjectState.Queries;
using ProjectManager.Application.ProjectTasks.Commands.AddUserToTask;
using ProjectManager.Application.ProjectTasks.Commands.CreateTasks;
using ProjectManager.Application.ProjectTasks.Commands.DeleteTaskById;
using ProjectManager.Application.ProjectTasks.Commands.ModifyTask;
using ProjectManager.Application.ProjectTasks.Commands.RemoveUserFromTask;
using ProjectManager.Application.ProjectTasks.Queries.GetAllTasksByProjectId;
using ProjectManager.Application.ProjectTasks.Queries.GetTaskById;
using ProjectManager.Application.TableParameters;
using ProjectManager.Application.TaskPriority.Queries.GetAllTaskPriorities;
using ProjectManager.Application.TaskState.Queries;
using ProjectManager.Application.TaskType.Queries.GetAllTaskTypes;
using ProjectManager.Application.Users.Queries.GetUsersNotInProject;
using ProjectManager.Application.Users.Queries.GetUsersNotInTask;
using ProjectManager.Application.Users.Queries.GetUsersOfProject;
using ProjectManager.Application.Validators.Projects;
using ProjectManager.Application.Validators.ProjectTasks;
using System.Security.Claims;

namespace ProjectManager.MVC.Controllers
{
    public class TaskManagerController : BaseController
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View("Index");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> IndexContent(int projectId = 0, List<string> errors = null)
        {
            ViewBag.Error = errors;
            ViewBag.ProjectId = projectId;

            ProjectDetailsVM selectedProject = await Mediator.Send(new GetProjectByIdQuery { Id = projectId });
            ViewBag.SelectedProject = selectedProject;

            List<UsersNotInVM> usersNoIt = await Mediator.Send(new GetUsersNotInProjectQuery { ProjectId = projectId });
            ViewBag.UserNotIn = usersNoIt;

            List<ProjectStateVM> projectStates = await Mediator.Send(new GetAllProjectStateQuery());
            ViewBag.ProjectStates = projectStates;

            return View("IndexContent");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProject(int projectId, List<string> errors = null)
        {
            ViewBag.Error = errors;
            ViewBag.ProjectId = projectId;

            ProjectDetailsVM selectedProject = await Mediator.Send(new GetProjectByIdQuery { Id = projectId });
            ViewBag.SelectedProject = selectedProject;

            List<UsersNotInVM> usersNoIt = await Mediator.Send(new GetUsersNotInProjectQuery { ProjectId = projectId });
            ViewBag.UserNotIn = usersNoIt;

            List<ProjectStateVM> projectStates = await Mediator.Send(new GetAllProjectStateQuery());
            ViewBag.ProjectStates = projectStates;

            return View("EditProject");
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> UsersOfProject(int projectId)
        {
            ViewBag.ProjectId = projectId;
            ProjectDetailsVM selectedProject = await Mediator.Send(new GetProjectByIdQuery { Id = projectId });
            ViewBag.SelectedProject = selectedProject;
            return View("UsersOfProject");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetUsersOfProject(int projectId = 0, DataTablesParameters parameters = null)
        {
            Tuple<int, IList<UserOfProjectVM>> result = await Mediator.Send(new GetUsersOfProjectQuery { ProjectId = projectId, Parameters = parameters});

            var v = new
            {
                Draw = parameters.Draw,
                RecordsFiltered = result.Item1,
                RecordsTotal = result.Item1,
                Data = result.Item2
            };

            return Ok(v);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> TaskManagerMenu()
        {

            List<ProjectVM> projects = await Mediator.Send(new GetAllProjetsByUserIdQuery { UserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) });

            ViewBag.Projects = projects;

            return View("../Shared/_TaskManagerMenu");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ProjectDetails(int projectId)
        {
            ViewBag.ProjectId = projectId;

            ProjectDetailsVM selectedProject = await Mediator.Send(new GetProjectByIdQuery { Id = projectId });
            ViewBag.SelectedProject = selectedProject;

            List<UsersNotInVM> usersNoIt = await Mediator.Send(new GetUsersNotInProjectQuery { ProjectId = projectId });
            ViewBag.UserNotIn = usersNoIt;

            List<ProjectStateVM> projectStates = await Mediator.Send(new GetAllProjectStateQuery());
            ViewBag.ProjectStates = projectStates;

            return View("ProjectDetails");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProject(int projectId, [FromForm] UpdateProjectDTO dto)
        {
            dto.Id = projectId;

            UpdateProjectDTOValidator validator = new UpdateProjectDTOValidator();
            ValidationResult validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }

                return await EditProject(projectId, errors);
            }

            int result = await Mediator.Send(new ModifyProjectCommand { DTO = dto});

            return await EditProject(projectId);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreateProject(List<string> errors = null)
        {
            ViewBag.Error = errors;

            List<ProjectVM> projects = await Mediator.Send(new GetAllProjetsByUserIdQuery { UserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)) });

            ViewBag.Projects = projects;

            return View("CreateProject");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> CreateTask(int projectId = 0, List<string> errors = null)
        {
            ViewBag.Error = errors;
            ViewBag.ProjectId = projectId;

            List<TaskTypeVM> taskTypes = await Mediator.Send(new GetAllTaskTypesQuery());
            ViewBag.TaskTypes = taskTypes;
            List<PriorityVM> priorities = await Mediator.Send(new GetAllTaskPrioritiesQuery());
            ViewBag.Priorities = priorities;

            return View("CreateTask");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTask(int projectId, CreateTaskDTO dto)
        {
            dto.CreatorId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            dto.ProjectId = projectId;

            CreateTaskDTOValidator validator = new CreateTaskDTOValidator();
            ValidationResult validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }

                return await CreateTask(dto.ProjectId, errors);
            }

            await Mediator.Send(new CreateTaskCommand { DTO = dto });

            return await IndexContent(dto.ProjectId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateProject(CreateProjectDTO dto)
        {
            dto.CreatorId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            CreateProjectDTOValidator validator = new CreateProjectDTOValidator();
            ValidationResult validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }

                return await CreateProject(errors);
            }

            await Mediator.Send(new CreateProjectCommand {DTO = dto });

            return Ok("");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> TaskDetails(int taskId)
        {
            ViewBag.TaskId = taskId;

            ProjectTaskVM projectTask = await Mediator.Send(new GetTaskByIdQuery { ProjectTaskId = taskId });

            ViewBag.ProjectTask = projectTask;
            List<TaskTypeVM> taskTypes = await Mediator.Send(new GetAllTaskTypesQuery());
            ViewBag.TaskTypes = taskTypes;
            List<PriorityVM> priorities = await Mediator.Send(new GetAllTaskPrioritiesQuery());
            ViewBag.Priorities = priorities;
            List<TaskStateVM> taskStates = await Mediator.Send(new GetAllTaskStateQuery());
            ViewBag.TaskStates = taskStates;

            return View("TaskDetails");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditTask(int taskId, List<string> errors = null)
        {
            ViewBag.Error = errors;

            ViewBag.TaskId = taskId;

            ProjectTaskVM projectTask = await Mediator.Send(new GetTaskByIdQuery { ProjectTaskId = taskId });

            ViewBag.ProjectTask = projectTask;
            ViewBag.ProjectTaskId = taskId;

            ViewBag.ProjectTaskId = taskId;

            List<UsersNotInVM> usersNoIt = await Mediator.Send(new GetusersNotInTaskQuery { ProjectTaskId = taskId });
            ViewBag.UserNotIn = usersNoIt;

            List<TaskTypeVM> taskTypes = await Mediator.Send(new GetAllTaskTypesQuery());
            ViewBag.TaskTypes = taskTypes;
            List<PriorityVM> priorities = await Mediator.Send(new GetAllTaskPrioritiesQuery());
            ViewBag.Priorities = priorities;
            List<TaskStateVM> taskStates = await Mediator.Send(new GetAllTaskStateQuery());
            ViewBag.TaskStates = taskStates;

            return View("EditTask");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditTask(int taskId ,[FromForm] UpdateTaskDTO dto)
        {
            dto.ModifiedBy = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            dto.Id = taskId;

            UpdateTaskDTOValidator validator = new UpdateTaskDTOValidator();
            ValidationResult validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }

                return await EditTask(taskId, errors );
            }

            await Mediator.Send(new ModifyTaskCommand { DTO = dto});

            return await EditTask(taskId);
        }

        [HttpPost]
        public async Task<IActionResult> ReturnData(int projectId = 0, DataTablesParameters parameters = null)
        {
            Tuple<int,List<ProjectTaskRowVM>> result = await Mediator.Send(new GetAllTasksByProjectIdQuery { ProjectId = projectId, Parameters = parameters });

            var v = new {
                Draw = parameters.Draw,
                RecordsFiltered = result.Item1,
                RecordsTotal = result.Item1,
                Data = result.Item2
            };

            return Ok(v);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            await Mediator.Send(new DeleteTaskByIdCommand { TaskId = taskId });

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserFromTask(int userId, int taskId)
        {
            int result = await Mediator.Send(new RemoveUserFromTaskCommand { UserId = userId, TaskId = taskId});

            return result > 0 ? Ok() : BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveUserFromProject(int userId, int projectId)
        {
            int result = await Mediator.Send(new RemoveUserFromProjectCommand { UserId = userId, ProjectId = projectId });

            return result > 0 ? Ok() : BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> AddOneUserToProject(int userId, int projectId)
        {
            int result = await Mediator.Send(new AddUserToProjectCommand { UserId = userId, ProjectId = projectId});

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
                        AddUserToProjectCommand addUserToProjectQuery = new AddUserToProjectCommand
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
                        AddUserToTaskCommand addProjectToTaskQuery = new AddUserToTaskCommand
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
