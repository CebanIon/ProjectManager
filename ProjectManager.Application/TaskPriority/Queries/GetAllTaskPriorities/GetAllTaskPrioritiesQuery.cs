using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.TaskPriority.Queries.GetAllTaskPriorities
{
    public class GetAllTaskPrioritiesQuery : IRequest<List<PriorityVM>>
    {
    }

    public class GetAllTaskPrioritiesHandler : IRequestHandler<GetAllTaskPrioritiesQuery, List<PriorityVM>>
    {
        private readonly IProjectManagerDbContext _context;

        public GetAllTaskPrioritiesHandler(IProjectManagerDbContext context)
        {
            this._context = context;
        }

        public async Task<List<PriorityVM>> Handle(GetAllTaskPrioritiesQuery request, CancellationToken cancellationToken)
        {
            List<PriorityVM> result = await _context
                .Priority.Select(x =>
                    new PriorityVM
                    {
                        Id = x.Id,
                        Name = x.Name,
                        PriorityValue = x.PriorityValue
                    }).ToListAsync(cancellationToken);

            return result;
        }
    }
}
