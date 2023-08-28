using MediatR;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Projects.Commands.CreateProject
{
    public class CreateProjectCommand : IRequest<int>
    {
        public int CreatorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? ProjectStartDate { get; set; }

        public DateTime? ProjectEndDate { get; set; }
    }

    public class CreateProjectHandler : IRequestHandler<CreateProjectCommand, int>
    {
        private readonly IProjectManagerDbContext _context;
        public CreateProjectHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }
        public async Task<int> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
        {
            Project project = new Project();

            project.Name = request.Name;
            project.Description = request.Description;
            project.ProjectStartDate = request.ProjectStartDate;
            project.ProjectEndDate = request.ProjectEndDate;
            project.CreatedBy = request.CreatorId;
            project.ProjectState = _context.ProjectStates.FirstOrDefault(x => x.Name == "Frozen");
            project.ProjectStateId = project.ProjectState.Id;
            project.LastModified = DateTime.UtcNow;
            project.Created = DateTime.UtcNow;
            project.LastModifiedBy = request.CreatorId;


            await _context.Projects.AddAsync(project, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            UserProject userProject = new UserProject
            {
                User = _context.Users.FirstOrDefault(x => x.Id == request.CreatorId),
                Project = project,
            };

            await _context.UserProject.AddAsync(userProject, cancellationToken);

            return await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
