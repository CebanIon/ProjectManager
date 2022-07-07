using ProjectManager.Domain.Common;

namespace ProjectManager.Domain.Entities
{
    public class ProjectTaskState : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
    }
}
