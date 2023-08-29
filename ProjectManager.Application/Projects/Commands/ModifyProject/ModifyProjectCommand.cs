using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Application.DTO_s.Projects;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Projects.Commands.ModifyProject
{
    public class ModifyProjectCommand : IRequest<int>
    {
        public UpdateProjectDTO DTO { get; set; }
    }
    public class ModifyProjectHandler : IRequestHandler<ModifyProjectCommand, int>
    {
        private readonly IProjectManagerDbContext _context;

        public ModifyProjectHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(ModifyProjectCommand request, CancellationToken cancellationToken)
        {
            if (request == null || request.DTO == null)
            {
                return 0;
            }

            Project project = await _context.Projects.Where(x => x.Id == request.DTO.Id).FirstOrDefaultAsync(cancellationToken);

            project.Name = request.DTO.Name;
            project.Description = request.DTO.Description;
            project.IsDeleted = request.DTO.IsDeleted;
            project.ProjectStartDate = request.DTO.ProjectStartDate;
            project.ProjectEndDate = request.DTO.ProjectEndDate;
            project.ProjectStateId = request.DTO.ProjectStateId;
            project.ProjectState = await _context.ProjectStates.Where(x => x.Id == project.ProjectStateId).FirstOrDefaultAsync(cancellationToken);

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
