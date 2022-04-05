
namespace ProjectManager.Domain.Common
{
    public class AuditableEntity : BaseEntity
    {
        public int CreatedBy { get; set; }  
        public DateTime Created { get; set; }   
        public int? LastModifiedBy { get; set; }    
        public DateTime? LastModified { get; set; } 
    }
}
