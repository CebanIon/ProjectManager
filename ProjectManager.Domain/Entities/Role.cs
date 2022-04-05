using ProjectManager.Domain.Common;

namespace ProjectManager.Domain.Entities
{
    public class Role: BaseEntity
    {
        public Role(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public string? Description { get; set; }
        public List<User> Users { get; set; } = new List<User>(); 
    }
}
