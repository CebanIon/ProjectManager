using FluentValidation;
using ProjectManager.Application.DTO_s.ProjectTasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Validators.ProjectTasks
{
    public class UpdateTaskDTOValidator : AbstractValidator<UpdateTaskDTO>
    {
        public UpdateTaskDTOValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.ModifiedBy).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().WithMessage("Task name cannot be empty")
                .MinimumLength(5).WithMessage("Task name should contain at least 5 characters");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Task description cannot be empty")
                .MinimumLength(20).WithMessage("Task description should contain at least 20 characters");
            RuleFor(x => x.TaskTypeId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.TaskStateId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.PriorityId).NotEmpty().GreaterThan(0);
        }
    }
}
