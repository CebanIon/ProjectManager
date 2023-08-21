using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.Projects.Queries.GetAllProjectsByUserId;
using ProjectManager.Application.Roles.Queries.GetRoleByUserId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Projects.Queries.GetAllProjectsOfUser
{
    public class GetAllProjetsByUserIdQuery : IRequest<List<ProjectVM>>
    {
        public int UserId { get; set; }
    }

    public class GetAllProjetsByUserIdHandler : IRequestHandler<GetAllProjetsByUserIdQuery, List<ProjectVM>>
    {
        private readonly IProjectManagerDbContext _context;
        public GetAllProjetsByUserIdHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }
        public async Task<List<ProjectVM>> Handle(GetAllProjetsByUserIdQuery request, CancellationToken cancellationToken)
        {
            List<ProjectVM> result = await _context.Projects
                .Include(x => x.ProjectState)
                .Include(x => x.UserProjects.
                Where(x => x.UserId == request.UserId))
                .Select(x => new ProjectVM { Id = x.Id, Name = x.Name, State = x.ProjectState.Name }).
                ToListAsync(cancellationToken);

            return result;
        }
    }
}
