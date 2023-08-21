using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.ProjectTasks.Queries.GetInProgressTasksByUserId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.ProjectTasks.Queries.GetPendingTasksByUserId
{
    public class GetPendingTasksByUserIdQuery : IRequest<IList<PendingTasksVM>>
    {
        public int UserId { get; set; }
    }

    public class GetPendingTasksByUserIdHandler : IRequestHandler<GetPendingTasksByUserIdQuery, IList<PendingTasksVM>>
    {
        private readonly IProjectManagerDbContext _context;

        public GetPendingTasksByUserIdHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }

        public async Task<IList<PendingTasksVM>> Handle(GetPendingTasksByUserIdQuery request, CancellationToken cancellationToken)
        {
            IList<PendingTasksVM> result = await _context.ProjectTasks
                .Include(x => x.UserProjectTasks)
                .Include(x => x.TaskState)
                .Where(x => x.TaskState.Name == "Pending")
                .Where(x => x.UserProjectTasks.Any(y => y.UserId == request.UserId))
                .Select(x => new PendingTasksVM
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync(cancellationToken);


            return result;
        }
    }
}
