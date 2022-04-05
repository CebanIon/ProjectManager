using ProjectManager.Domain.Common;

namespace ProjectManager.Domain.Entities
{
    public class ProjectState : BaseEntity
    {
        public ProjectState(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Project Project { get; set; } = null!;
    }
}
