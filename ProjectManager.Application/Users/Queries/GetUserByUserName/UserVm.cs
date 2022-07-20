
namespace ProjectManager.Application.Users.Queries.GetUserByUserName
{
    public class UserVm
    {
        public int Id { get; set; }    
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public string Role { get; set; }
        public int RoleId { get; set; }
    }
}
