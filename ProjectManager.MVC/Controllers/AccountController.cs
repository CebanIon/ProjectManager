using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManager.Application.Users.Queries.GetUserByUserName;
using System.Security.Claims;

namespace ProjectManager.MVC.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string userName, string password)
        {
            try
            {
                UserVm userVm = await Mediator.Send(new GetUserByUserNameQuery { UserName = userName, Password = password });

                if (userVm == null || !userVm.IsEnabled)
                {
                    ModelState.TryAddModelError("IncorrectLogin", "Non-existent or disabled user");
                    return View("Login");
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
                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var userPrincipal = new ClaimsPrincipal(new[] { claimsIdentity });
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);

                    return RedirectToAction("Index", "User");
                }
            }
            catch (Exception)
            {
                return View("Login");
            }
        }
    }
}
