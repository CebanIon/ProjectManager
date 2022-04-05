using ProjectManager.Domain.Common;

namespace ProjectManager.Domain.Entities
{
    public class ProjectToUser 
    {
        public int UserId { get; set; } 
        public int ProjectId { get; set; }  
        public int ProjectUserRoleId { get; set; }

        public User User { get; set; } = null!;
        public Project Project { get; set; } = null!;
        public ProjectUserRole ProjectUserRole { get; set; } = null!;
    }
}
