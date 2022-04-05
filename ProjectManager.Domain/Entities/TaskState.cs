using ProjectManager.Domain.Common;

namespace ProjectManager.Domain.Entities
{
    public class TaskState : BaseEntity
    {
        public TaskState(string name)
        {
            Name = name;
        }
        public string Name { get; set; }
        public string? Description { get; set; }
        public List<Task> Tasks { get; set; } = new List<Task>();
    }
}
