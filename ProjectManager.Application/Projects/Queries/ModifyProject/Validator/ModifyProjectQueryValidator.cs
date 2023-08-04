using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Projects.Queries.ModifyProject.Validator
{
    public class ModifyProjectQueryValidator : AbstractValidator<ModifyProjectQuery>
    {
        public ModifyProjectQueryValidator() 
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().WithMessage("Task name cannot be empty")
                .MinimumLength(5).WithMessage("Task name too short");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Task description cannot be empty")
                .MinimumLength(30).WithMessage("Task description too short");
            RuleFor(x => x.ProjectStateId).NotEmpty().GreaterThan(0);
        }
    }
}
