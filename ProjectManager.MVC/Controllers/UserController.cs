using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Users.Queries.CreateUser;
using ProjectManager.Application.Users.Queries.GetAllUsers;
using ProjectManager.Application.Users.Queries.GetUserById;
using ProjectManager.Application.Users.Queries.GetUserByUserName;
using ProjectManager.Application.Users.Queries.UpdateUser;
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
        public async Task<IActionResult> GetAllUsers()
        {
            List<UserTableRowVM> result = await Mediator.Send(new GetAllUsersQuery());

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> EditUser(int userId)
        {
            UserUpdateVM userDetails = await Mediator.Send(new GetUserByIdQuery { UserId = userId});

            ViewBag.UserDetails = userDetails;
            ViewBag.UserId = userId;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUserPost(int userId, [FromForm]UpdateUserQuery userQuery)
        {
            userQuery.Id = userId;
            userQuery.LastModifiedBy = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            await Mediator.Send(userQuery);

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(CreateUserQuery createUserQuery)
        {
            createUserQuery.CreatorId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            int result = await Mediator.Send(createUserQuery);

            return RedirectToAction("Index");
        }
    }
}
