using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.TaskType.Queries.GetAllTaskTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.ProjectState.Queries
{
    public class GetAllProjectStateQuery : IRequest<List<ProjectStateVM>>
    {
    }

    public class GetAllProjectStateHandler : IRequestHandler<GetAllProjectStateQuery, List<ProjectStateVM>>
    {
        private readonly IProjectManagerDbContext _context;

        public GetAllProjectStateHandler(IProjectManagerDbContext context)
        {
            this._context = context;
        }

        public async Task<List<ProjectStateVM>> Handle(GetAllProjectStateQuery request, CancellationToken cancellationToken)
        {
            List<ProjectStateVM> result = await _context
               .ProjectStates.Select(x =>
                   new ProjectStateVM
                   {
                       Id = x.Id,
                       Name = x.Name,
                       Description = x.Description
                   }).ToListAsync(cancellationToken);

            return result;
        }
    }
}
