using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.ProjectTasks.Commands.RemoveUserFromTask
{
    public class RemoveUserFromTaskCommand : IRequest<int>
    {
        public int UserId { get; set; }
        public int TaskId { get; set; }
    }

    public class RemoveUserFromTaskHandler : IRequestHandler<RemoveUserFromTaskCommand, int>
    {
        private readonly IProjectManagerDbContext _context;
        public RemoveUserFromTaskHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(RemoveUserFromTaskCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return 0;
            }

            UserProjectTask userProjectTask = await _context.UserProjectTask
                .Where(x => x.ProjectTaskId == request.TaskId && x.UserId == request.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            _context.UserProjectTask.Remove(userProjectTask);

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
