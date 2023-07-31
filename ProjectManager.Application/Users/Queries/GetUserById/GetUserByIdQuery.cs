using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Users.Queries.GetUserById
{
    public class GetUserByIdQuery : IRequest<UserUpdateVM>
    {
        public int UserId { get; set; }
    }
    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserUpdateVM>
    {
        private readonly IProjectManagerDbContext _context;

        public GetUserByIdHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }

        public async Task<UserUpdateVM> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            UserUpdateVM userVM = await _context.Users.Where(x => x.Id == request.UserId)
                .Include(x => x.Role)
                .Select(x => new UserUpdateVM
                {
                    UserName = x.UserName,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    IsEnabled = x.IsEnabled,
                    RoleId = x.RoleId,
                    RoleName = x.Role.Name

                })
                .FirstOrDefaultAsync(cancellationToken);

            await _context.ProjectTasks.Include(x => x.UserProjectTasks)
                .Where(x => x.UserProjectTasks.Any(j => j.UserId == request.UserId))
                .Include(x => x.Project)
                .ForEachAsync(x => 
                {
                    userVM.ProjectTasks.Add(new Tuple<int, Tuple<string, string>> 
                    (
                        item1 : x.Id,
                        item2 : new Tuple<string, string>
                            (
                                item1 : x.Name,
                                item2 : x.Project.Name
                            )
                    ));
                },
                cancellationToken);

            await _context.Projects.Include(x => x.UserProjects)
                .Where(j => j.UserProjects.Any(x => x.UserId == request.UserId))
                .ForEachAsync(x => 
                {
                    userVM.Projects.Add(new Tuple<int, string>(item1: x.Id, item2: x.Name));
                },
                cancellationToken);

            return userVM;
        }
    }
}
