using ProjectManager.Domain.Common;

namespace ProjectManager.Domain.Entities
{
    public class File : AuditableEntity
    {
        public string FileName { get; set; }
        public byte[] FileData { get; set; }
        public bool IsDeleted { get; set; }
        public int ProjectTaskId { get; set; }
        public ProjectTask ProjectTask { get; set; }
    }
}
