using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Projects.Queries.RemoveUserFromProject
{
    public class RemoveUserFromProjectQuery : IRequest<int>
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
    }

    public class RemoveUserFromProjectHandler : IRequestHandler<RemoveUserFromProjectQuery, int>
    {
        private readonly IProjectManagerDbContext _context;

        public RemoveUserFromProjectHandler(IProjectManagerDbContext context)
        {
            this._context = context;
        }

        public async Task<int> Handle(RemoveUserFromProjectQuery request, CancellationToken cancellationToken)
        {
            UserProject userProject = await _context.UserProject
                .Where(x => x.ProjectId == request.ProjectId && x.UserId == request.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            _context.UserProject.Remove(userProject);

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
