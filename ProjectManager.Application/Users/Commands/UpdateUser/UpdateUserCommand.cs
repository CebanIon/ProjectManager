using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.DTO_s.Users;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Users.Commands.UpdateUser
{
    public class UpdateUserCommand : IRequest<int>
    {
        public UpdateUserDTO DTO { get; set; }
    }

    public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, int>
    {
        private readonly IProjectManagerDbContext _context;

        public UpdateUserHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            User user = await _context.Users.Where(x => x.Id == request.DTO.Id).FirstOrDefaultAsync(cancellationToken);

            user.UserName = request.DTO.UserName;
            user.FirstName = request.DTO.FirstName;
            user.LastName = request.DTO.LastName;
            user.Email = request.DTO.Email;
            user.IsEnabled = request.DTO.IsEnabled;
            user.RoleId = request.DTO.RoleId;
            user.Role = await _context.Roles.Where(x => x.Id == user.RoleId).FirstOrDefaultAsync(cancellationToken);
            user.LastModified = DateTime.UtcNow;
            user.LastModifiedBy = request.DTO.LastModifiedBy;

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
