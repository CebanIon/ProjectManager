using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.ProjectTasks.Queries.GetTaskById
{
    public class ProjectTaskVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TaskTypeId { get; set; }
        public int TaskStateId { get; set; }
        public int ProjectId { get; set; }
        public int PriorityId { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }

        public List<Tuple<int, string>> Users { get; set; } = new List<Tuple<int, string>>();
    }
}
