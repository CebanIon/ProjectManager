using IdentityModel;
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

        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(string username, string password)
        {
            try
            {
                UserVm userVm = await Mediator.Send(new GetUserByUserNameQuery { UserName = username, Password = password });

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
                        new Claim("LastName", userVm.LastName ?? ""),
                        new Claim(ClaimTypes.Email, userVm.Email),
                        new Claim(ClaimTypes.Role, userVm.Role),

                    };

                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                    claimsIdentity.AddClaims(userClaims);

                    var principal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    return LocalRedirect("/Home");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return View("_Login");
            }
        }

        [HttpGet]
        public async Task<IActionResult> LogoffAsync()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
