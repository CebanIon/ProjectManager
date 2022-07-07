using System.Security.Claims;

using ProjectManager.Application.Common.Interfaces;

namespace ProjectManager.WebUI.Services;

public class CurrentUserService : ICurrentUserService
{
    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        if (int.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
        {
            UserId = userId;
        }

        if (int.TryParse(httpContextAccessor.HttpContext?.User?.FindFirstValue("RoleId"), out int roleId))
        {
            RoleId = roleId;
        }

        UserName = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
        FirstName = httpContextAccessor.HttpContext?.User?.FindFirstValue("FirstName");
        LastName = httpContextAccessor.HttpContext?.User?.FindFirstValue("LastName");
        Email = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Email);
    }

    public int UserId { get; }
    public int RoleId { get; set; }
    public string UserName { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
}
