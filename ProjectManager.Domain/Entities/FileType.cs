using ProjectManager.Domain.Common;
namespace ProjectManager.Domain.Entities
{
    public class FileType : BaseEntity
    {
        public string Type { get; set; }
        public List<File> FilesByFileType { get; set; } 
    }
}
