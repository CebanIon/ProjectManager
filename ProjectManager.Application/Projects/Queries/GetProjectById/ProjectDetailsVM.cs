using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Projects.Queries.GetProjectById
{
    public class ProjectDetailsVM
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProjectStateId { get; set; }
        public bool IsDeleted { get; set; }
        public string StateName { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
        public List<Tuple<int, string>> Users { get; set; } = new List<Tuple<int, string>>();
    }
}
