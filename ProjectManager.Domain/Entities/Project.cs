using ProjectManager.Domain.Common;

namespace ProjectManager.Domain.Entities
{
    public class Project : AuditableEntity
    {
        public string Name { get; set; }    
        public string Description { get; set; } 
        public int ProjectStateId { get; set; } 
        public bool IsDeleted { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
        public ProjectState ProjectState { get; set; }

        public List<ProjectTask> ProjectTasks { get; set; }
        public List<UserProject> UserProjects { get; set; }
    }
}
