using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Users.Queries.CreateUser.Validator
{
    public class CreateUserQueryValidator : AbstractValidator<CreateUserQuery>
    {
        public CreateUserQueryValidator() 
        {
            RuleFor(x => x.CreatorId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username cannot be empty")
                .MinimumLength(5).WithMessage("Username to short")
                .MaximumLength(30).WithMessage("Username to long");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be empty")
                .MinimumLength(5).WithMessage("Password to short")
                .MaximumLength(30).WithMessage("Password to long");
            RuleFor(x => x.Email).EmailAddress().WithMessage("Not an email address");
            RuleFor(x => x.RoleId).NotEmpty().GreaterThan(0);
        }
    }
}
