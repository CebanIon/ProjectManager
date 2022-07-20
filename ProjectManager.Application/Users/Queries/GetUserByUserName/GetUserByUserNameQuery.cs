using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProjectManager.Application.Common.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace ProjectManager.Application.Users.Queries.GetUserByUserName
{
    public class GetUserByUserNameQuery : IRequest<UserVm>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class GetUserByUserNameQueryHandler : IRequestHandler<GetUserByUserNameQuery, UserVm>
    {
        private readonly IProjectManagerDbContext _context;

        public GetUserByUserNameQueryHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }

        public async Task<UserVm> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
        {
            //TO DO: costume exception with message ("incomplete credentials")
            if ((request.UserName == null || request.UserName == string.Empty) || (request.Password == null || request.Password == string.Empty))
            {
                throw new Exception(); 
            }
            string encrypted = "";
            using (SHA256 hash = SHA256.Create())
            {
                encrypted = String.Concat(hash
                  .ComputeHash(Encoding.UTF8.GetBytes(request.Password))
                  .Select(item => item.ToString("x2")));
            }

            //TO DO: costume exception with message ("password not matching")
            if (encrypted != await _context.Users.Where(x => x.UserName.ToUpper() == request.UserName.ToUpper() && x.IsEnabled)
                .Select(x => x.Password).FirstOrDefaultAsync(cancellationToken))
            {
                throw new Exception();
            }

            return await _context.Users.Where(x => x.UserName.ToUpper() == request.UserName.ToUpper() && x.IsEnabled)
                .Select(x => new UserVm
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Role = x.Role.Name,
                    RoleId = x.RoleId,
                }).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
