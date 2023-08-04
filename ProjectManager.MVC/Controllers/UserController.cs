﻿using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Roles.Queries.GetAllRoles;
using ProjectManager.Application.Roles.Queries.GetRoleByUserId;
using ProjectManager.Application.TableParameters;
using ProjectManager.Application.Users.Queries.CreateUser;
using ProjectManager.Application.Users.Queries.CreateUser.Validator;
using ProjectManager.Application.Users.Queries.GetAllUsers;
using ProjectManager.Application.Users.Queries.GetUserById;
using ProjectManager.Application.Users.Queries.GetUserByUserName;
using ProjectManager.Application.Users.Queries.UpdateUser;
using ProjectManager.Application.Users.Queries.UpdateUser.Validator;
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
            List<UserTableRowVM> result = await Mediator.Send(new GetAllUsersQuery { Parameters = parameters });

            return Ok(result);
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

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserPost(int userId, [FromForm]UpdateUserQuery userQuery)
        {
            userQuery.Id = userId;
            userQuery.LastModifiedBy = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            UpdateUserQueryValidator validator = new UpdateUserQueryValidator();
            ValidationResult validationResult = validator.Validate(userQuery);

            if (!validationResult.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }

                return RedirectToAction("EditUser", new { userId = userId,  errors = errors });
            }

            await Mediator.Send(userQuery);

            return RedirectToAction("EditUser", new { userId  = userId });
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create(List<string> errors = null)
        {
            ViewBag.Error = errors;
            List<RoleVM> roles = await Mediator.Send(new GetAllRoleQuery());
            ViewBag.Roles = roles;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(CreateUserQuery createUserQuery)
        {
            createUserQuery.CreatorId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            CreateUserQueryValidator validator = new CreateUserQueryValidator();
            ValidationResult validationResult = validator.Validate(createUserQuery);

            if (!validationResult.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (var error in validationResult.Errors)
                {
                    errors.Add(error.ErrorMessage);
                }

                return RedirectToAction("Create", new { errors  = errors });
            }

            int result = await Mediator.Send(createUserQuery);

            return RedirectToAction("Index");
        }
    }
}
