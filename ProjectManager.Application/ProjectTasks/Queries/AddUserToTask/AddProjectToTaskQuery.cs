using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.ProjectTasks.Queries.AddUserToTask
{
    public class AddProjectToTaskQuery : IRequest<int>
    {
        public int TaskId { get; set; }

        public int UserId { get; set; }
    }

    public class AddProjectToTaskHandler : IRequestHandler<AddProjectToTaskQuery, int>
    {
        private readonly IProjectManagerDbContext _context;

        public AddProjectToTaskHandler(IProjectManagerDbContext context) 
        {
            this._context = context;
        }

        public async Task<int> Handle(AddProjectToTaskQuery request, CancellationToken cancellationToken)
        {
            if (await _context.Users.Where(x => x.Id == request.UserId).Include(x => x.UserProjectTasks)
                .Where(x => x.UserProjectTasks
                .Any(x => x.ProjectTaskId == request.TaskId))
                .AnyAsync(cancellationToken))
            {
                return -1;
            }
            
            UserProjectTask userProject = new UserProjectTask
            {
                ProjectTask = await _context.ProjectTasks.Where(x => x.Id == request.TaskId).FirstOrDefaultAsync(cancellationToken),
                User = await _context.Users.Where(x => x.Id == request.UserId).FirstOrDefaultAsync(cancellationToken)
            };

            await _context.UserProjectTask.AddAsync(userProject, cancellationToken);

            return await _context.SaveChangesAsync(cancellationToken);
    }
    }
}
