#region usings

using CompareHare.Api.Features.Authentication.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Interceptors;

#endregion

namespace CompareHare.Api.Features.Authentication.RequestHandlers.LogIn
{
    public class LogInModelValidator : AbstractValidator<LogInModel>, IValidatorInterceptor
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

        public ValidationResult? AfterValidation(ActionExecutingContext actionExecutingContext, IValidationContext validationContext)
        {
            return null;
        }

        public IValidationContext? BeforeValidation(ActionExecutingContext actionExecutingContext, IValidationContext validationContext)
        {
            return null;
        }
    }
}
