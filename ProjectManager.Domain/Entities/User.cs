using ProjectManager.Domain.Common;

namespace ProjectManager.Domain.Entities
{
    public class User : AuditableEntity
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }

        public List<UserProject> PersonClubs { get; set; }
    }
}
