using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Entities
{
    public class UserProjectTask
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int ProjectTaskId { get; set; }
        public ProjectTask ProjectTask { get; set; }
    }
}
