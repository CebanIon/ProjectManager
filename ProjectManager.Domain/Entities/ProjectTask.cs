using ProjectManager.Domain.Common;

namespace ProjectManager.Domain.Entities
{
    public class ProjectTask : AuditableEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int TaskTypeId { get; set; }
        public int TaskStateId { get; set; }
        /// <summary>
        /// This field specifies the date at which the task ends.
        /// The date at which task start will be deducted from Created field in AuditableEntity.
        /// </summary>
        public DateTime? TaskEndDate { get; set; }
        public ProjectTaskType TaskType { get; set; }
        public ProjectTaskState TaskState { get; set; }
    }
}
