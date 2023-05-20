using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.Users.Queries.GetUserByUserName;
using ProjectManager.Application.Roles.Queries.GetRoleByUserId;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Roles.Queries.GetRoleByUserId
{
    public class GetRoleByUserIdQuery : IRequest<RoleVM>
    {
        public int UserId { get; set; }
    }
    public class GetRoleByUserIdQueryHandler : IRequestHandler<GetRoleByUserIdQuery, RoleVM>
    {
        private readonly IProjectManagerDbContext _context;
        public GetRoleByUserIdQueryHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }

        public async Task<RoleVM> Handle(GetRoleByUserIdQuery request, CancellationToken cancellationToken)
        {
            RoleVM role = await _context.Users
                .Where(x => x.Id == request.UserId)
                .Select(x => new RoleVM
                {
                    Id = x.Role.Id,
                    Name = x.Role.Name,
                    Description = x.Role.Description,
                })
                .FirstOrDefaultAsync(cancellationToken);

            return role;
        }
    }
}
