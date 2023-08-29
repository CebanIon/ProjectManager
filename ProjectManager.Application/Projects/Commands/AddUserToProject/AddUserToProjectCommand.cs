using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Projects.Commands.AddUserToProject
{
    public class AddUserToProjectCommand : IRequest<int>
    {
        public int ProjectId { get; set; }

        public int UserId { get; set; }
    }

    public class AddUserToProjectHandler : IRequestHandler<AddUserToProjectCommand, int>
    {
        private readonly IProjectManagerDbContext _context;

        public AddUserToProjectHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(AddUserToProjectCommand request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return 0;
            }

            if (await _context.Users.Where(x => x.Id == request.UserId).Include(x => x.UserProjects)
            .Where(x => x.UserProjects
            .Any(x => x.ProjectId == request.ProjectId))
            .AnyAsync(cancellationToken))
            {
                return -1;
            }

            UserProject userProject = new UserProject
            {
                Project = await _context.Projects.Where(x => x.Id == request.ProjectId).FirstOrDefaultAsync(cancellationToken),
                User = await _context.Users.Where(x => x.Id == request.UserId).FirstOrDefaultAsync(cancellationToken)
            };

            await _context.UserProject.AddAsync(userProject, cancellationToken);

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
