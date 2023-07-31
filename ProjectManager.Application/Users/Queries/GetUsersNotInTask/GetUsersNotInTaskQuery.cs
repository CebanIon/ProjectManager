using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Users.Queries.GetUsersNotInTask
{
    public class GetusersNotInTaskQuery : IRequest<List<UsersNotInVM>>
    {
        public int ProjectTaskId {  get; set; }
    }

    public class GetusersNotInTaskhandler : IRequestHandler<GetusersNotInTaskQuery, List<UsersNotInVM>>
    {
        public IProjectManagerDbContext _context;

        public GetusersNotInTaskhandler(IProjectManagerDbContext context)
        {
            _context = context;
        }

        public async Task<List<UsersNotInVM>> Handle(GetusersNotInTaskQuery request, CancellationToken cancellationToken)
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
