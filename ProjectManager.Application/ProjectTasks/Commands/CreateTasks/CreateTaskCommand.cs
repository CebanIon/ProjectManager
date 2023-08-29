using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.DTO_s.ProjectTasks;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.ProjectTasks.Commands.CreateTasks
{
    public class CreateTaskCommand : IRequest<int>
    {
        public CreateTaskDTO DTO { get; set; }
    }

    public class CreateTaskHandler : IRequestHandler<CreateTaskCommand, int>
    {
        private readonly IProjectManagerDbContext _context;
        public CreateTaskHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.DTO == null)
            {
                return 0;
            }

            ProjectTask projectTask = new ProjectTask();

            projectTask.Name = request.DTO.Name;
            projectTask.Description = request.DTO.Description;
            projectTask.TaskState = _context.ProjectTaskStates.FirstOrDefault(x => x.Name == "Pending");
            projectTask.TaskStateId = projectTask.TaskState.Id;
            projectTask.TaskTypeId = request.DTO.TaskTypeId;
            projectTask.TaskType = _context.ProjectTaskTypes.FirstOrDefault(x => x.Id == request.DTO.TaskTypeId);
            projectTask.TaskStartDate = request.DTO.TaskStartDate;
            projectTask.TaskEndDate = request.DTO.TaskEndDate;
            projectTask.CreatedBy = request.DTO.CreatorId;
            projectTask.PriorityId = request.DTO.PriorityId;
            projectTask.Priority = _context.Priority.FirstOrDefault(x => x.Id == request.DTO.PriorityId);
            projectTask.LastModified = DateTime.UtcNow;
            projectTask.Created = DateTime.UtcNow;
            projectTask.LastModifiedBy = request.DTO.CreatorId;
            projectTask.ProjectId = request.DTO.ProjectId;
            projectTask.Project = _context.Projects.FirstOrDefault(x => x.Id == request.DTO.ProjectId);

            await _context.ProjectTasks.AddAsync(projectTask, cancellationToken);

            return await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
