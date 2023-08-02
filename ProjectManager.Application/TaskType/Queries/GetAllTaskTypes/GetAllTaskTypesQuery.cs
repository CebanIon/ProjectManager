using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.TaskPriority.Queries.GetAllTaskPriorities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.TaskType.Queries.GetAllTaskTypes
{
    public class GetAllTaskTypesQuery : IRequest<List<TaskTypeVM>>
    {
    }

    public class GetAllTaskTypesHandler : IRequestHandler<GetAllTaskTypesQuery, List<TaskTypeVM>>
    {
        private readonly IProjectManagerDbContext _context;

        public GetAllTaskTypesHandler(IProjectManagerDbContext context)
        {
            this._context = context;
        }

        public async Task<List<TaskTypeVM>> Handle(GetAllTaskTypesQuery request, CancellationToken cancellationToken)
        {
            List<TaskTypeVM> result = await _context
               .ProjectTaskTypes.Select(x =>
                   new TaskTypeVM
                   {
                       Id = x.Id,
                       Name = x.Name,
                       Description = x.Description
                   }).ToListAsync(cancellationToken);

            return result;
        }
    }
}
