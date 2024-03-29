﻿using ProjectManager.Domain.Common;

namespace ProjectManager.Domain.Entities
{
    public class ProjectState : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Project> Projects { get; set; } = new List<Project>();
        
    }
}
