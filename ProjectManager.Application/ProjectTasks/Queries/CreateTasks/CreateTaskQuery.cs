using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.ProjectTasks.Queries.CreateTasks
{
    public class CreateTaskQuery : IRequest<int>
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

    public class CreateTaskHandler : IRequestHandler<CreateTaskQuery, int>
    {
        private readonly IProjectManagerDbContext _context;
        public CreateTaskHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateTaskQuery request, CancellationToken cancellationToken)
        {
            ProjectTask projectTask = new ProjectTask();

            projectTask.Name = request.Name;
            projectTask.Description = request.Description;
            projectTask.TaskState = _context.ProjectTaskStates.FirstOrDefault(x => x.Name == "Pending");
            projectTask.TaskStateId = projectTask.TaskState.Id;
            projectTask.TaskTypeId = request.TaskTypeId;
            projectTask.TaskType = _context.ProjectTaskTypes.FirstOrDefault(x => x.Id == request.TaskTypeId);
            projectTask.TaskStartDate = request.TaskStartDate;
            projectTask.TaskEndDate = request.TaskEndDate;
            projectTask.CreatedBy = request.CreatorId;
            projectTask.PriorityId = request.PriorityId;
            projectTask.Priority = _context.Priority.FirstOrDefault(x => x.Id == request.PriorityId);
            projectTask.LastModified = DateTime.UtcNow;
            projectTask.Created = DateTime.UtcNow;
            projectTask.LastModifiedBy = request.CreatorId;
            projectTask.ProjectId = request.ProjectId;
            projectTask.Project = _context.Projects.FirstOrDefault(x => x.Id == request.ProjectId);

            await _context.ProjectTasks.AddAsync(projectTask, cancellationToken);

            return await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
