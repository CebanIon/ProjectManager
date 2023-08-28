using FluentValidation;
using ProjectManager.Application.DTO_s.Users;
using ProjectManager.Application.Users.Commands.CreateUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Application.Validators.Users
{
    public class CreateUserDTOValidator : AbstractValidator<CreateUserDTO>
    {
        public CreateUserDTOValidator()
        {
            RuleFor(x => x.CreatorId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.UserName).NotEmpty().WithMessage("Username cannot be empty")
                .MinimumLength(5).WithMessage("Username should contain at least 5 characters")
                .MaximumLength(30).WithMessage("Username should contain maximum 30 characters");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password cannot be empty")
                .MinimumLength(5).WithMessage("Password should contain at least 5 characters")
                .MaximumLength(30).WithMessage("Password should contain maximum 30 characters");
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email address is required").EmailAddress().WithMessage("Not an email address");
            RuleFor(x => x.RoleId).NotEmpty().GreaterThan(0);
            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName is required").MaximumLength(50).WithMessage("FirstName max length is 50 characters");
            RuleFor(x => x.LastName).MaximumLength(50).WithMessage("LastName max length is 50 characters");
        }
    }
}
