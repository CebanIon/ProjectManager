using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.ProjectTasks.Queries.GetUpcomingTasksDueByUserId
{
    public class UpcommingTasksVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int DaysLeft { get; set; }
    }
}
