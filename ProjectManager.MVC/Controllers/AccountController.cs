using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Roles.Queries.GetRoleByUserId;
using ProjectManager.Application.Users.Queries.GetUserByUserName;
using ProjectManager.Domain.Entities;
using System.Security.Claims;

namespace ProjectManager.MVC.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Login()
        {
            return View("_Login");
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(string userName, string password)
        {
            try
            {
                UserVm userVm = await Mediator.Send(new GetUserByUserNameQuery { UserName = userName, Password = password });

                if (userVm == null || !userVm.IsEnabled)
                {
                    ModelState.TryAddModelError("IncorrectLogin", "Non-existent or disabled user");
                    return View("_Login");
                }
                else
                {
                    List<Claim> userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, userVm.Id.ToString()),
                        new Claim(ClaimTypes.Name, userVm.UserName),
                        new Claim("FirstName", userVm.FirstName),
                        new Claim("LastName", userVm.LastName),
                        new Claim(ClaimTypes.Email, userVm.Email),
                        new Claim(ClaimTypes.Role, userVm.Role),

                    };
                    RoleVM userRolesList = await Mediator.Send(new GetRoleByUserIdQuery { UserId = userVm.Id });
                    userClaims.Add(new Claim(ClaimTypes.Role, userRolesList.Name));

                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "User");
                }
            }
            catch (Exception)
            {
                return View("_Login");
            }
        }

        [HttpGet]
        public async Task<IActionResult> LogoffAsync()
        {
            await HttpContext.SignOutAsync();
            return View("_Login");
        }
    }
}
