using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTO_s.ProjectTasks
{
    public class UpdateTaskDTO
    {
        public int ModifiedBy { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
        public int TaskTypeId { get; set; }
        public int TaskStateId { get; set; }
        public int PriorityId { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }
    }
}
