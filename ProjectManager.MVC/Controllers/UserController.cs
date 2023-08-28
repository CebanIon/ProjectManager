using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.DTO_s.Users;
using ProjectManager.Application.Roles.Queries.GetAllRoles;
using ProjectManager.Application.Roles.Queries.GetRoleByUserId;
using ProjectManager.Application.TableParameters;
using ProjectManager.Application.Users.Commands.CreateUser;
using ProjectManager.Application.Users.Commands.UpdateUser;
using ProjectManager.Application.Users.Queries.GetAllUsers;
using ProjectManager.Application.Users.Queries.GetUserById;
using ProjectManager.Application.Users.Queries.GetUserByUserName;
using ProjectManager.Application.Validators.Users;
using System.Security.Claims;

namespace ProjectManager.MVC.Controllers
{
    public class UserController : BaseController
    {
        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Index()
        {
            return View("_Index");
        }

        [HttpPost]
        public async Task<IActionResult> GetAllUsers(DataTablesParameters parameters = null)
        {
            Tuple<int,List<UserTableRowVM>> result = await Mediator.Send(new GetAllUsersQuery { Parameters = parameters });

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
        public async Task<IActionResult> UserDetails(int userId)
        {
            UserUpdateVM userDetails = await Mediator.Send(new GetUserByIdQuery { UserId = userId });

            ViewBag.UserDetails = userDetails;
            ViewBag.UserId = userId;

            List<RoleVM> roles = await Mediator.Send(new GetAllRoleQuery());
            ViewBag.Roles = roles;

            return View("UserDetails");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditUser(int userId, List<string> errors = null)
        {
            ViewBag.Error = errors;
            UserUpdateVM userDetails = await Mediator.Send(new GetUserByIdQuery { UserId = userId});

            ViewBag.UserDetails = userDetails;
            ViewBag.UserId = userId;

            List<RoleVM> roles = await Mediator.Send(new GetAllRoleQuery());
            ViewBag.Roles = roles;

            return View("EditUser");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(int userId, [FromForm] UpdateUserDTO dto)
        {
            dto.Id = userId;
            dto.LastModifiedBy = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            UpdateUserDTOValidator validator = new UpdateUserDTOValidator();
            ValidationResult validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }

                return await EditUser(userId, errors);
            }

            await Mediator.Send(new UpdateUserCommand { DTO = dto});

            return await EditUser(userId);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(List<string> errors = null)
        {
            ViewBag.Error = errors;
            List<RoleVM> roles = await Mediator.Send(new GetAllRoleQuery());
            ViewBag.Roles = roles;

            return View("Create");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateUserDTO dto)
        {
            dto.CreatorId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            CreateUserDTOValidator validator = new CreateUserDTOValidator();
            ValidationResult validationResult = validator.Validate(dto);

            if (!validationResult.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }

                return await Create(errors);
            }

            int result = await Mediator.Send(new CreateUserCommand { DTO = dto});

            return Index();
        }
    }
}
