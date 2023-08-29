using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.DTO_s.ProjectTasks;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.ProjectTasks.Commands.ModifyTask
{
    public class ModifyTaskCommand : IRequest<int>
    {
        public UpdateTaskDTO DTO { get; set; }
    }
    public class ModifyTaskhandler : IRequestHandler<ModifyTaskCommand, int>
    {
        private readonly IProjectManagerDbContext _context;

        public ModifyTaskhandler(IProjectManagerDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(ModifyTaskCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.DTO == null)
            {
                return 0;
            }

            ProjectTask projectTask = await _context.ProjectTasks.FirstOrDefaultAsync(x => x.Id == request.DTO.Id, cancellationToken);

            if (projectTask == default)
            {
                return -1;
            }

            projectTask.LastModified = DateTime.UtcNow;
            projectTask.LastModifiedBy = request.DTO.ModifiedBy;
            projectTask.Name = request.DTO.Name;
            projectTask.Description = request.DTO.Description;
            projectTask.TaskTypeId = request.DTO.TaskTypeId;
            projectTask.TaskType = await _context.ProjectTaskTypes.FirstOrDefaultAsync(x => x.Id == projectTask.TaskTypeId, cancellationToken);
            projectTask.Project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == projectTask.ProjectId, cancellationToken);
            projectTask.TaskStartDate = request.DTO.TaskStartDate;
            projectTask.TaskEndDate = request.DTO.TaskEndDate;
            projectTask.PriorityId = request.DTO.PriorityId;
            projectTask.Priority = await _context.Priority.FirstOrDefaultAsync(x => x.Id == projectTask.PriorityId, cancellationToken);
            projectTask.TaskStateId = request.DTO.TaskStateId;
            projectTask.TaskState = await _context.ProjectTaskStates.FirstOrDefaultAsync(x => x.Id == projectTask.TaskStateId, cancellationToken);

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
