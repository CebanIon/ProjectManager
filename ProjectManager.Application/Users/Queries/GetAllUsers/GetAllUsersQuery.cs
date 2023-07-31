using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.Users.Queries.GetUserByUserName;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<List<UserTableRowVM>>
    {
    }

    public class GetAllUserHandler : IRequestHandler<GetAllUsersQuery, List<UserTableRowVM>>
    {
        private readonly IProjectManagerDbContext _context;

        public GetAllUserHandler(IProjectManagerDbContext context) 
        {
            this._context = context;
        }

        public async Task<List<UserTableRowVM>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
        {
            List<UserTableRowVM> result = await _context.Users.Select(x => new UserTableRowVM
            {
                        Id = x.Id,
                        UserName = x.UserName,
                        Email = x.Email,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        IsEnabled = x.IsEnabled,
                        Role = x.Role.Name,
                        RoleId = x.RoleId
                    }).ToListAsync(cancellationToken);

            return result;
        }
    }
}
