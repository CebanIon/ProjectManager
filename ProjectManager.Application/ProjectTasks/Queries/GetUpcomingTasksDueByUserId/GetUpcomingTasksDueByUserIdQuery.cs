using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.ProjectTasks.Queries.GetUpcomingTasksDueByUserId
{
    public class GetUpcomingTasksDueByUserIdQuery : IRequest<IList<UpcommingTasksVM>>
    {
        public int UserId { get; set; }
    }

    public class GetUpcomingTasksDueByUserIdHandler : IRequestHandler<GetUpcomingTasksDueByUserIdQuery, IList<UpcommingTasksVM>>
    {
        private readonly IProjectManagerDbContext _context;

        public GetUpcomingTasksDueByUserIdHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }

        public async Task<IList<UpcommingTasksVM>> Handle(GetUpcomingTasksDueByUserIdQuery request, CancellationToken cancellationToken)
        {
            var projects = await _context.ProjectTasks
                .Include(x => x.UserProjectTasks)
                .Where(x => x.UserProjectTasks.Any(y => y.UserId == request.UserId))
                .Where(x => x.TaskState.Name == "In Progress" || x.TaskState.Name == "Pending")
                .Where(x => x.TaskEndDate != null)
                .ToListAsync(cancellationToken);

            IList<UpcommingTasksVM> result = projects.Select(x => new UpcommingTasksVM
                {
                    Id = x.Id,
                    Name = x.Name,
                    DaysLeft = (x.TaskEndDate - DateTime.UtcNow).Value.Days + 1
                })
                .OrderBy(x => x.DaysLeft)
                .ToList();

            return result;
        }
    }
}
