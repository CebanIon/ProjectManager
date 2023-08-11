using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.Projects.Queries.GetAllProjectsByUserId;
using ProjectManager.Application.ProjectTasks.Queries.GetTaskById;
using ProjectManager.Application.TableParameters;
using ProjectManager.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ProjectManager.Application.ProjectTasks.Queries.GetAllTasksByProjectId
{
    public class GetAllTasksByProjectIdQuery : IRequest<Tuple<int,List<ProjectTaskRowVM>>>
    {
        public int ProjectId { get; set; }

        public DataTablesParameters Parameters { get; set; }
    }

    public class GetAllTasksByProjectIdHandler : IRequestHandler<GetAllTasksByProjectIdQuery, Tuple<int, List<ProjectTaskRowVM>>>
    {
        private readonly IProjectManagerDbContext _context;
        public GetAllTasksByProjectIdHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }
        public async Task<Tuple<int, List<ProjectTaskRowVM>>> Handle(GetAllTasksByProjectIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                string oderColumn = request.Parameters.Columns[request.Parameters.Order[0].Column].Name;
                string toFind = request.Parameters.Search.Value ?? "";

                List<ProjectTaskRowVM> result = default;

                result = await _context.ProjectTasks.Where(x => x.ProjectId == request.ProjectId)
                .Include(x => x.TaskType)
                .Include(x => x.Priority)
                .Include(x => x.TaskState)
                .Where(x => x.Name.Contains(toFind)
                    || x.Description.Contains(toFind)
                    || x.Priority.Name.Contains(toFind)
                    || x.TaskType.Name.Contains(toFind)
                    || x.TaskState.Name.Contains(toFind))
                .OrderByExtension(oderColumn, request.Parameters.Order[0].Dir)
                .Skip(request.Parameters.Start)
                .Take(request.Parameters.Length)
                .Select(x => new ProjectTaskRowVM
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    TaskType = x.TaskType.Name,
                    Priority = x.Priority.Name,
                    TaskState = x.TaskState.Name
                })
                .ToListAsync();

                int total = await _context.ProjectTasks.Where(x => x.ProjectId == request.ProjectId)
                .Include(x => x.TaskType)
                .Include(x => x.Priority)
                .Include(x => x.TaskState)
                .Where(x => x.Name.Contains(toFind)
                    || x.Description.Contains(toFind)
                    || x.Priority.Name.Contains(toFind)
                    || x.TaskType.Name.Contains(toFind)
                    || x.TaskState.Name.Contains(toFind))
                    .CountAsync(cancellationToken);



                return new Tuple<int, List<ProjectTaskRowVM>>(item1 : total, item2 : result);
            }
            catch (Exception e)
            {
                return default;
            }
        }

    }

    public static class LinqHelper
    {
        public static IQueryable<T> OrderByExtension<T>(this IQueryable<T> source, string ordering,string dir, params object[] values)
        {
            var type = typeof(T);
            var property = type.GetProperty(ordering, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable),dir == "asc" ? "OrderBy" : "OrderByDescending", new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));
            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}
