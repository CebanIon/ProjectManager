using FluentValidation;
using ProjectManager.Application.Projects.Commands.CreateProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Projects.Commands.CreateProject.Validator
{
    public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator()
        {
            RuleFor(x => x.CreatorId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().WithMessage("Task name cannot be empty")
                .MinimumLength(5).WithMessage("Task name should contain at least 5 characters");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Task description cannot be empty")
                .MinimumLength(30).WithMessage("Task description should contain at least 30 characterst");
        }
    }
}
