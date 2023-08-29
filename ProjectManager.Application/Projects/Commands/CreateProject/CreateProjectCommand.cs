using MediatR;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.DTO_s.Projects;
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
       public CreateProjectDTO DTO { get; set; }
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
            if (request == null || request.DTO == null)
            {
                return 0;    
            }

            Project project = new Project();

            project.Name = request.DTO.Name;
            project.Description = request.DTO.Description;
            project.ProjectStartDate = request.DTO.ProjectStartDate;
            project.ProjectEndDate = request.DTO.ProjectEndDate;
            project.CreatedBy = request.DTO.CreatorId;
            project.ProjectState = _context.ProjectStates.FirstOrDefault(x => x.Name == "Frozen");
            project.ProjectStateId = project.ProjectState.Id;
            project.LastModified = DateTime.UtcNow;
            project.Created = DateTime.UtcNow;
            project.LastModifiedBy = request.DTO.CreatorId;


            await _context.Projects.AddAsync(project, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            UserProject userProject = new UserProject
            {
                User = _context.Users.FirstOrDefault(x => x.Id == request.DTO.CreatorId),
                Project = project,
            };

            await _context.UserProject.AddAsync(userProject, cancellationToken);

            return await _context.SaveChangesAsync(cancellationToken);

        }
    }
}
