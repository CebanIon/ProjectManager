using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.ProjectTasks.Queries.DeleteTaskById
{
    public class DeleteTaskByIdQuery: IRequest<int>
    {
        public int TaskId { get; set; }
    }

    public class DeleteTaskByIdHandler : IRequestHandler<DeleteTaskByIdQuery, int>
    {
        private readonly IProjectManagerDbContext _context;
        public DeleteTaskByIdHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(DeleteTaskByIdQuery request, CancellationToken cancellationToken)
        {

            List<User> users = await _context.Users.Include(x => x.UserProjectTasks).ToListAsync(cancellationToken);

            if (users != default)
            {
                foreach (var user in users) 
                {
                    foreach (var userTask in user.UserProjectTasks.Where(x => x.ProjectTaskId == request.TaskId))
                    {
                        user.UserProjectTasks.Remove(userTask);
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            ProjectTask projectTask = await _context.ProjectTasks.SingleOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken);

            if (projectTask != null)
            {

                _context.ProjectTasks.Remove(projectTask);
            }

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
