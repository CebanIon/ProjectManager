using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTO_s.ProjectTasks
{
    public class CreateTaskDTO
    {
        public int CreatorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TaskTypeId { get; set; }
        public int ProjectId { get; set; }
        public int PriorityId { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }
    }
}
