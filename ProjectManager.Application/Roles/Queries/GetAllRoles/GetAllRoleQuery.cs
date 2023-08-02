using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.Roles.Queries.GetRoleByUserId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Roles.Queries.GetAllRoles
{
    public class GetAllRoleQuery : IRequest<List<RoleVM>>
    {
    }

    public class GetAllRoleHandler : IRequestHandler<GetAllRoleQuery, List<RoleVM>>
    {
        private readonly IProjectManagerDbContext _context;

        public GetAllRoleHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }

        public async Task<List<RoleVM>> Handle(GetAllRoleQuery request, CancellationToken cancellationToken)
        {
            List<RoleVM> result = await _context.Roles
                .Select(x => new RoleVM 
                { 
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description
                })
                .ToListAsync(cancellationToken);

            return result;
        }
    }
}
