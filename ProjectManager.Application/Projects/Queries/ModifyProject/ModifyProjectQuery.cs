using MediatR;
using Microsoft.EntityFrameworkCore;
using ProjectManager.Application.Common.Interfaces;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Projects.Queries.ModifyProject
{
    public class ModifyProjectQuery : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ProjectStateId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? ProjectStartDate { get; set; }
        public DateTime? ProjectEndDate { get; set; }
    }
    public class ModifyProjectHandler : IRequestHandler<ModifyProjectQuery, int>
    {
        private readonly IProjectManagerDbContext _context;

        public ModifyProjectHandler(IProjectManagerDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(ModifyProjectQuery request, CancellationToken cancellationToken)
        {
            Project project = await _context.Projects.Where(x => x.Id == request.Id).FirstOrDefaultAsync(cancellationToken);

            project.Name = request.Name;
            project.IsDeleted = request.IsDeleted;
            project.ProjectStartDate = request.ProjectStartDate;
            project.ProjectEndDate = request.ProjectEndDate;
            project.ProjectStateId = request.ProjectStateId;
            project.ProjectState = await _context.ProjectStates.Where(x => x.Id == project.ProjectStateId).FirstOrDefaultAsync(cancellationToken);

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
