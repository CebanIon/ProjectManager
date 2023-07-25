using ProjectManager.Domain.Common;

namespace ProjectManager.Domain.Entities
{
    public class ProjectTask : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int TaskTypeId { get; set; }
        public int TaskStateId { get; set; }
        public int ProjectId { get; set; }
        public int PriorityId { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }
        public ProjectTaskType TaskType { get; set; }
        public ProjectTaskState TaskState { get; set; }
        public Project Project { get; set; }
        public Priority Priority { get; set; }
        public List<UserProjectTask> UserProjectTasks { get; set; }
    }
}
