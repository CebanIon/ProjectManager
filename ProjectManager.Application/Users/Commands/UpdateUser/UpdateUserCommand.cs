using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
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
        public int Id { get; set; }
        public int LastModifiedBy { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsEnabled { get; set; }
        public int RoleId { get; set; }
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
            User user = await _context.Users.Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

            user.UserName = request.UserName;
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Email = request.Email;
            user.IsEnabled = request.IsEnabled;
            user.RoleId = request.RoleId;
            user.Role = await _context.Roles.Where(x => x.Id == user.RoleId).FirstOrDefaultAsync(cancellationToken);
            user.LastModified = DateTime.UtcNow;
            user.LastModifiedBy = request.LastModifiedBy;

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
