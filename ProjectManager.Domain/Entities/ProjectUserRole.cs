using ProjectManager.Domain.Common;

namespace ProjectManager.Domain.Entities
{
    public class ProjectUserRole : BaseEntity
    {
        public ProjectUserRole(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<ProjectToUser> ProjectToUsers { get; set; } = new List<ProjectToUser>();
    }
}
