using FluentValidation;
using ProjectManager.Application.DTO_s.ProjectTasks;
using ProjectManager.Application.ProjectTasks.Commands.CreateTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Validators.ProjectTasks
{
    public class CreateTaskDTOValidator : AbstractValidator<CreateTaskDTO>
    {
        public CreateTaskDTOValidator()
        {
            RuleFor(x => x.CreatorId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().WithMessage("Task name cannot be empty")
                .MinimumLength(5).WithMessage("Task name should contain at least 5 characters");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Task description cannot be empty")
                .MinimumLength(20).WithMessage("Task description should contain at least 20 characters");
            RuleFor(x => x.TaskTypeId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.PriorityId).NotEmpty().GreaterThan(0);
        }
    }
}
