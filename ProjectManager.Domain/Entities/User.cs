using ProjectManager.Domain.Common;

namespace ProjectManager.Domain.Entities
{
    public class User : AuditableEntity
    {
        public User(string name, string password, string firstName, string lastName, string email)
        {
            Name = name;
            Password = password;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
        }

        public string Name { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;
        public List<ProjectToUser> ProjectToUsers { get; set; } = new List<ProjectToUser>();

    }
}
