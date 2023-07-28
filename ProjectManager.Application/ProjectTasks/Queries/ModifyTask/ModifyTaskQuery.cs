using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.ProjectTasks.Queries.ModifyTask
{
    public class ModifyTaskQuery : IRequest<int>
    {
        public int ModifiedBy { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int TaskTypeId { get; set; }
        public int ProjectId { get; set; }
        public int PriorityId { get; set; }
        public DateTime? TaskStartDate { get; set; }
        public DateTime? TaskEndDate { get; set; }
    }
    public class ModifyTaskhandler : IRequestHandler<ModifyTaskQuery, int>
    {
        private readonly IProjectManagerDbContext _context;

        public ModifyTaskhandler(IProjectManagerDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(ModifyTaskQuery request, CancellationToken cancellationToken)
        {
            ProjectTask projectTask =await  _context.ProjectTasks.FirstOrDefaultAsync( x => x.Id == request.Id, cancellationToken);

            if (projectTask == default)
            {
                return -1;
            }

            projectTask.LastModified = DateTime.UtcNow;
            projectTask.LastModifiedBy = request.ModifiedBy;
            projectTask.Name = request.Name;
            projectTask.Description = request.Description;
            projectTask.TaskTypeId = request.TaskTypeId;
            projectTask.TaskType = await _context.ProjectTaskTypes.FirstOrDefaultAsync( x => x.Id ==  projectTask.TaskTypeId, cancellationToken);
            projectTask.ProjectId = request.ProjectId;
            projectTask.Project = await _context.Projects.FirstOrDefaultAsync(x => x.Id == projectTask.ProjectId, cancellationToken);
            projectTask.TaskStartDate = request.TaskStartDate;
            projectTask.TaskEndDate = request.TaskEndDate;
            projectTask.PriorityId = request.PriorityId;
            projectTask.Priority = await _context.Priority.FirstOrDefaultAsync(x => x.Id == projectTask.PriorityId, cancellationToken);

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
