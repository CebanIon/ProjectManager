using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.DTO_s.Projects;
using ProjectManager.Application.Projects.Queries.GetAllProjectsByUserId;
using ProjectManager.Application.TableParameters;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Projects.Queries.GetAllProjects
{
    public class GetAllProjectsQuery : IRequest<List<ViewSimpleProjectDTO>>
    {
        public string? Filter { get; set; }

        public GetAllProjectsQuery(string? filter)
        {
            Filter = filter;
        }
    }

    public class GetAllProjectsQueryHandler : IRequestHandler<GetAllProjectsQuery, List<ViewSimpleProjectDTO>>
    {
        private readonly IProjectManagerDbContext _context;

        public GetAllProjectsQueryHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }

        public async Task<List<ViewSimpleProjectDTO>> Handle(GetAllProjectsQuery request, CancellationToken cancellationToken)
        {
            var result = new List<ViewSimpleProjectDTO>();

            if (request.Filter != null)
            {
                result = await _context.Projects
                  .Where(x => x.Name.ToLower().Contains(request.Filter.ToLower()))
                  .Include(x => x.ProjectState)
                  .Include(x => x.UserProjects)
                  .Select(x => new ViewSimpleProjectDTO
                  {
                      Id = x.Id,
                      Name = x.Name,
                      Description = x.Description,
                      StartDate = x.ProjectStartDate.Value.ToString("dd/mm/yyyy"),
                  }).
                  ToListAsync(cancellationToken);
                return result;
            }

            result = await _context.Projects
                   .Include(x => x.ProjectState)
                   .Include(x => x.UserProjects)
                   .Select(x => new ViewSimpleProjectDTO
                   {
                       Id = x.Id,
                       Name = x.Name,
                       Description = x.Description,
                       StartDate = x.ProjectStartDate.Value.ToString("dd/mm/yyyy"),
                   }).
                   ToListAsync(cancellationToken);
            return result;
        }
    }
}
