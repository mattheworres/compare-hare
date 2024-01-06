using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.Filters;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Interceptors;

namespace CompareHare.Api.Features.Shared.Validation.Interfaces
{
    public class DefaultValidatorInterceptor<T> : AbstractValidator<T>, IValidatorInterceptor
    {
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
