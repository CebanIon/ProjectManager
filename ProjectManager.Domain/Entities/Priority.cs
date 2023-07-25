using ProjectManager.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Entities
{
    public class Priority : BaseEntity
    {
        public string Name { get; set; }

        public int PriorityValue { get; set; }

        public List<ProjectTask> ProjectTasks { get; set; } = new List<ProjectTask>();
    }
}
