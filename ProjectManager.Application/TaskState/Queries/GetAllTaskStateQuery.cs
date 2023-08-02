using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.ProjectState.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.TaskState.Queries
{
    public class GetAllTaskStateQuery : IRequest<List<TaskStateVM>>
    {
    }

    public class GetAllTaskStateHandler : IRequestHandler<GetAllTaskStateQuery, List<TaskStateVM>>
    {
        private readonly IProjectManagerDbContext _context;

        public GetAllTaskStateHandler(IProjectManagerDbContext context)
        {
            this._context = context;
        }

        public async Task<List<TaskStateVM>> Handle(GetAllTaskStateQuery request, CancellationToken cancellationToken)
        {
            List<TaskStateVM> result = await _context
               .ProjectTaskStates.Select(x =>
                   new TaskStateVM
                   {
                       Id = x.Id,
                       Name = x.Name,
                       Description = x.Description
                   }).ToListAsync(cancellationToken);

            return result;
        }
    }
}
