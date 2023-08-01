using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Projects.Queries.GetProjectById
{
    public class GetProjectByIdQuery : IRequest<ProjectDetailsVM>
    {
        public int Id { get; set; }
    }

    public class GetProjectByIdhandler : IRequestHandler<GetProjectByIdQuery, ProjectDetailsVM>
    {
        private readonly IProjectManagerDbContext _context;

        public GetProjectByIdhandler(IProjectManagerDbContext context)
        {
            this._context = context;
        }

        public async Task<ProjectDetailsVM> Handle(GetProjectByIdQuery request, CancellationToken cancellationToken)
        {
            ProjectDetailsVM projectVM = await _context.Projects.Include(x => x.ProjectState)
                .Where(x => x.Id == request.Id)
                .Select(x => new ProjectDetailsVM 
                {
                    Name = x.Name,
                    Description = x.Description,
                    ProjectStateId  = x.ProjectStateId,
                    IsDeleted = x.IsDeleted,
                    StateName = x.ProjectState.Name,
                    ProjectStartDate = x.ProjectStartDate,
                    ProjectEndDate = x.ProjectEndDate
                })
                .FirstOrDefaultAsync(cancellationToken);

            _context.Projects
                .Where(x => x.Id == request.Id)
                .Include(x => x.UserProjects)
                .ThenInclude(x => x.User)
                .FirstOrDefault()
                .UserProjects
                .ForEach( x => 
                {
                    projectVM.Users.Add(new Tuple<int, string>(item1 : x.UserId, item2 : x.User.UserName));
                });


            return projectVM;
        }
    }
}
