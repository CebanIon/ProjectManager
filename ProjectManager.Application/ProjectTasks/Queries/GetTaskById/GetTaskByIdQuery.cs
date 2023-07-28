using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.ProjectTasks.Queries.GetTaskById
{
    public class GetTaskByIdQuery : IRequest<ProjectTaskVM>
    {
        public int ProjectTaskId { get; set; }
    }
    public class GetTaskByIdHandler : IRequestHandler<GetTaskByIdQuery, ProjectTaskVM>
    {
        private readonly IProjectManagerDbContext _context;
        public GetTaskByIdHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }
        public async Task<ProjectTaskVM> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
        {
            ProjectTaskVM vm = await _context.ProjectTasks
                .Where(x => x.Id == request.ProjectTaskId)
                .Select(x => new ProjectTaskVM 
                { 
                    Id = x.Id,
                    Name = x.Name,
                    ProjectId = x.ProjectId,
                    Description = x.Description,
                    TaskTypeId = x.TaskTypeId,
                    TaskStateId = x.TaskStateId,
                    TaskStartDate = x.TaskStartDate,
                    TaskEndDate = x.TaskEndDate,
                    PriorityId = x.PriorityId,
                })
                .FirstOrDefaultAsync();

            return vm;
        }
    }
}
