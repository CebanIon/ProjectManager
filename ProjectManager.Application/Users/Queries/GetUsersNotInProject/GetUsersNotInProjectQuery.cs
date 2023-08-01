using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.Users.Queries.GetUsersNotInTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Users.Queries.GetUsersNotInProject
{
    public class GetUsersNotInProjectQuery : IRequest<List<UsersNotInVM>>
    {
        public int ProjectTaskId { get; set; }
    }

    public class GetUsersNotInProjectHandler : IRequestHandler<GetUsersNotInProjectQuery, List<UsersNotInVM>>
    {
        public IProjectManagerDbContext _context;

        public GetUsersNotInProjectHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }

        public async Task<List<UsersNotInVM>> Handle(GetUsersNotInProjectQuery request, CancellationToken cancellationToken)
        {
            List<UsersNotInVM> result = await _context.Users.Include(x => x.UserProjectTasks)
                .Where(x => !x.UserProjectTasks.Any(x => x.ProjectTaskId == request.ProjectTaskId))
                .Select(x => new UsersNotInVM
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    LastName = x.LastName
                })
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
