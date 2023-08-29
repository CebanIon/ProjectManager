using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;
using System.Text;
using System.Security.Cryptography;
using ProjectManager.Application.DTO_s.Users;

namespace ProjectManager.Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<int>
    {
        public CreateUserDTO DTO { get; set; }
    }

    public class CreateUserHandler : IRequestHandler<CreateUserCommand, int>
    {
        private readonly IProjectManagerDbContext _context;

        public CreateUserHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.DTO == null)
            {
                return 0;
            }

            string encrypted = "";
            using (SHA256 hash = SHA256.Create())
            {
                encrypted = string.Concat(hash
                  .ComputeHash(Encoding.UTF8.GetBytes(request.DTO.Password))
                  .Select(item => item.ToString("x2")));
            };

            User user = new User
            {
                CreatedBy = request.DTO.CreatorId,
                Created = DateTime.UtcNow,
                UserName = request.DTO.UserName,
                Password = encrypted,
                Email = request.DTO.Email,
                FirstName = request.DTO.FirstName,
                RoleId = request.DTO.RoleId,
                LastName = request.DTO.LastName,
                Role = await _context.Roles.Where(x => x.Id == request.DTO.RoleId).FirstOrDefaultAsync(cancellationToken),
                IsEnabled = true
            };

            await _context.Users.AddAsync(user, cancellationToken);

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
