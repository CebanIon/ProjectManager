using FluentValidation;
using ProjectManager.Application.Users.Commands.UpdateUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Users.Commands.UpdateUser.Validator
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
            RuleFor(x => x.LastModifiedBy).NotEmpty().GreaterThan(0);
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username cannot be empty")
                .MinimumLength(5).WithMessage("Username should contain at least 5 characters")
                .MaximumLength(30).WithMessage("Username should contain at maximum 30 characters");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Not an email address");
            RuleFor(x => x.RoleId).NotEmpty().GreaterThan(0);
        }
    }
}
