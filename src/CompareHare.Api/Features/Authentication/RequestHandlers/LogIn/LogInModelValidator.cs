#region usings

using CompareHare.Api.Features.Authentication.Models;
using FluentValidation;

#endregion

namespace CompareHare.Api.Features.Authentication.RequestHandlers.LogIn
{
    public class LogInModelValidator : AbstractValidator<LogInModel>
    {
        public LogInModelValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress().WithMessage("Email must be a valid email address")
                .Length(1, 256).WithMessage("Email address must be between 1 and 256 characters.");
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required");
        }
    }
}
