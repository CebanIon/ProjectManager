using FluentValidation;
using ProjectManager.Application.Projects.Commands.ModifyProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Projects.Commands.ModifyProject.Validator
{
    public class ModifyProjectCommandValidator : AbstractValidator<ModifyProjectCommand>
    {
        public ModifyProjectCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().WithMessage("Task name cannot be empty")
                .MinimumLength(5).WithMessage("Task name should contain at least 5 characters");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Task description cannot be empty")
                .MinimumLength(30).WithMessage("Task description should contain at least 30 characterst");
            RuleFor(x => x.ProjectStateId).NotEmpty().GreaterThan(0);
        }
    }
}
