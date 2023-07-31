using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;
using System.Text;
using System.Security.Cryptography;

namespace ProjectManager.Application.Users.Queries.CreateUser
{
    public class CreateUserQuery : IRequest<int>
    {
        public int CreatorId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int RoleId { get; set; }
    }

    public class CreateUserHandler : IRequestHandler<CreateUserQuery, int>
    {
        private readonly IProjectManagerDbContext _context;

        public CreateUserHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateUserQuery request, CancellationToken cancellationToken)
        {
            string encrypted = "";
            using (SHA256 hash = SHA256.Create())
            {
                encrypted = String.Concat(hash
                  .ComputeHash(Encoding.UTF8.GetBytes(request.Password))
                  .Select(item => item.ToString("x2")));
            };

            User user = new User
            {
                CreatedBy = request.CreatorId,
                Created = DateTime.UtcNow,
                UserName = request.UserName,
                Password = encrypted,
                Email = request.Email,
                FirstName = request.FirstName,
                RoleId = request.RoleId,
                LastName = request.LastName,
                Role = await _context.Roles.Where(x => x.Id == request.RoleId).FirstOrDefaultAsync(cancellationToken),
                IsEnabled = true
            };

            await _context.Users.AddAsync(user, cancellationToken);

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
