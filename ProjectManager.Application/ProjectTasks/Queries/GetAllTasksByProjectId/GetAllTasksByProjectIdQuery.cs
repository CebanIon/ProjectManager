using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.Projects.Queries.GetAllProjectsByUserId;

namespace ProjectManager.Application.ProjectTasks.Queries.GetAllTasksByProjectId
{
    public class GetAllTasksByProjectIdQuery : IRequest<List<ProjectTaskRowVM>>
    {
        public int ProjectId { get; set; }
    }

    public class GetAllTasksByProjectIdHandler : IRequestHandler<GetAllTasksByProjectIdQuery, List<ProjectTaskRowVM>>
    {
        private readonly IProjectManagerDbContext _context;
        public GetAllTasksByProjectIdHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }
        public async Task<List<ProjectTaskRowVM>> Handle(GetAllTasksByProjectIdQuery request, CancellationToken cancellationToken)
        {
            List<ProjectTaskRowVM> result = await _context.ProjectTasks.Where(x => x.ProjectId == request.ProjectId)
                .Include(x => x.TaskType)
                .Select(x => new ProjectTaskRowVM 
                {
                    Id = x.Id,
                    Name = x.Name,
                    Description = x.Description,
                    TaskType = x.TaskType.Name
                }).ToListAsync();

            return result;
        }
    }
}
