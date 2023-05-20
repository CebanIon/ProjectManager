
namespace ProjectManager.Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        int UserId { get; }
        int RoleId { get; set; }
        bool IsAuthenticated { get; }
        string UserName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string Email { get; set; }
    }
}
