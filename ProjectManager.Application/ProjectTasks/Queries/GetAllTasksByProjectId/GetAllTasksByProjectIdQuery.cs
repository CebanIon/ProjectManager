using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.Projects.Queries.GetAllProjectsByUserId;

namespace ProjectManager.Application.ProjectTasks.Queries.GetAllTasksByProjectId
{
    public class GetAllTasksByProjectIdQuery : IRequest<List<ProjectTaskVM>>
    {
        public int ProjectId { get; set; }
    }

    public class GetAllTasksByProjectIdHandler : IRequestHandler<GetAllTasksByProjectIdQuery, List<ProjectTaskVM>>
    {
        private readonly IProjectManagerDbContext _context;
        public GetAllTasksByProjectIdHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }
        public async Task<List<ProjectTaskVM>> Handle(GetAllTasksByProjectIdQuery request, CancellationToken cancellationToken)
        {
            List<ProjectTaskVM> result = await _context.ProjectTasks.Where(x => x.ProjectId == request.ProjectId)
                .Include(x => x.TaskType)
                .Select(x => new ProjectTaskVM 
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
