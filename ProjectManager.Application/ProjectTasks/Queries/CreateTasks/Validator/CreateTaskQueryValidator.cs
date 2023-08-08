﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.ProjectTasks.Queries.CreateTasks.Validator
{
    public class CreateTaskQueryValidator : AbstractValidator<CreateTaskQuery>
    {
        public CreateTaskQueryValidator() 
        {
            RuleFor(x => x.CreatorId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.Name).NotEmpty().WithMessage("Task name cannot be empty")
                .MinimumLength(5).WithMessage("Task name too short");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Task description cannot be empty")
                .MinimumLength(20).WithMessage("Task description too short");
            RuleFor(x => x.TaskTypeId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.PriorityId).NotEmpty().GreaterThan(0);
        }
    }
}