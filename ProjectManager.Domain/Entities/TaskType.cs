using ProjectManager.Domain.Common;

namespace ProjectManager.Domain.Entities
{
    public class TaskType : BaseEntity
    {
        public TaskType(string name)
        {
            Name = name;    
        }
        public string Name { get; set; }
        public string? Description { get; set; }

        public List<Task> Tasks { get; set; } = new List<Task>();   
    }
}
