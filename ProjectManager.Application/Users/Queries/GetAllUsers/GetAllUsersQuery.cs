using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.TableParameters;
using ProjectManager.Application.Users.Queries.GetUserByUserName;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Users.Queries.GetAllUsers
{
    public class GetAllUsersQuery : IRequest<List<UserTableRowVM>>
    {
        public DataTablesParameters Parameters { get; set; }
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
            string oderColumn = request.Parameters.Columns[request.Parameters.Order[0].Column].Name;
            string toFind = request.Parameters.Search.Value ?? "";

            List<UserTableRowVM> result = await _context.Users
                .Include(x => x.Role)
                .Where(x => x.UserName.Contains(toFind)
                    || x.Email.Contains(toFind)
                    || x.FirstName.Contains(toFind)
                    || x.LastName.Contains(toFind)
                    || x.Role.Name.Contains(toFind))
                .OrderByExtension(oderColumn, request.Parameters.Order[0].Dir)
                .Skip(request.Parameters.Start)
                .Take(request.Parameters.Length)
                .Select(x => new UserTableRowVM
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
    public static class LinqHelper
    {
        public static IQueryable<T> OrderByExtension<T>(this IQueryable<T> source, string ordering, string dir, params object[] values)
        {
            var type = typeof(T);
            var property = type.GetProperty(ordering, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), dir == "asc" ? "OrderBy" : "OrderByDescending", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}
