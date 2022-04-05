using ProjectManager.Domain.Common;

namespace ProjectManager.Domain.Entities
{
    public class Project : AuditableEntity
    {
        public Project(string name,string description)
        {
            Name = name;
            Description = description; 
        }
        public string Name { get; set; }    
        public string Description { get; set; } 
        public int ProjectStateId { get; set; } 
        public bool IsDeleted { get; set; }

        public List<ProjectState> ProjectStates { get; set; } = new List<ProjectState>();
        public List<ProjectToUser> ProjectToUsers { get; set; } = new List<ProjectToUser>();
    }
}
