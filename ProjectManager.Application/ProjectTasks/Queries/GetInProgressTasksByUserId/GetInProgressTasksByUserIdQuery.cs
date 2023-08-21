using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.ProjectTasks.Queries.GetInProgressTasksByUserId
{
    public class GetInProgressTasksByUserIdQuery : IRequest<IList<InProgressTaskVM>>
    {
        public int UserId { get; set; }
    }

    public class GetInProgressTasksByUserIdHandler : IRequestHandler<GetInProgressTasksByUserIdQuery, IList<InProgressTaskVM>>
    {
        private readonly IProjectManagerDbContext _context;

        public GetInProgressTasksByUserIdHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }

        public async Task<IList<InProgressTaskVM>> Handle(GetInProgressTasksByUserIdQuery request, CancellationToken cancellationToken)
        {
            IList<InProgressTaskVM> result = await _context.ProjectTasks
                .Include(x => x.UserProjectTasks)
                .Where(x => x.UserProjectTasks.Any(y => y.UserId == request.UserId))
                .Select(x => new InProgressTaskVM
                {
                    Id = x.Id,
                    Name = x.Name
                })
                .ToListAsync(cancellationToken);


            return result;
        }
    }
}
