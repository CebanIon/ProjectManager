using ProjectManager.Domain.Common;

namespace ProjectManager.Domain.Entities
{
    public class Task : AuditableEntity
    {
        public Task(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TaskTypeId { get; set; }
        public int TaskStateId { get; set; }

        public TaskType TaskType { get; set; } = null!;
        public TaskState TaskState { get; set; } = null!;
    }
}
