using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalkFlow.Application.Commands.User.CreateUser
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.UserData.DisplayName)
                .NotEmpty().WithMessage("Display name is required")
                .Length(3, 50).WithMessage("Display name must be between 3 and 50 characters");

            RuleFor(x => x.UserData.Email)
                .EmailAddress().WithMessage("Invalid email format")
                .When(x => !string.IsNullOrEmpty(x.UserData.Email));

            RuleFor(x => x.UserData.Age)
                .InclusiveBetween(13, 120).WithMessage("Age must be between 13 and 120")
                .When(x => x.UserData.Age.HasValue);

            RuleFor(x => x.UserData.Gender)
                .Must(gender => string.IsNullOrEmpty(gender) ||
                    new[] { "Male", "Female", "Other", "PreferNotToSay" }.Contains(gender))
                .WithMessage("Invalid gender value");

            RuleFor(x => x.UserData.StrangerFilter)
                .SetValidator(new StrangerFilterValidator()!)
                .When(x => x.UserData.StrangerFilter != null);
        }
    }

    public class StrangerFilterValidator : AbstractValidator<DTOs.User.StrangerFilterDto>
    {
        public StrangerFilterValidator()
        {
            RuleFor(x => x.MinAge)
                .GreaterThanOrEqualTo(0).WithMessage("Min age must be at least 0");

            RuleFor(x => x.MaxAge)
                .LessThanOrEqualTo(100).WithMessage("Max age must be at most 100");

            RuleFor(x => x.MaxAge)
                .GreaterThanOrEqualTo(x => x.MinAge).WithMessage("Max age must be greater than or equal to min age");

            RuleFor(x => x.FindGender)
                .Must(genders => genders.All(g =>
                    new[] { "Male", "Female", "Other", "PreferNotToSay" }.Contains(g)))
                .WithMessage("Invalid gender values in filter");
        }
    }
}
