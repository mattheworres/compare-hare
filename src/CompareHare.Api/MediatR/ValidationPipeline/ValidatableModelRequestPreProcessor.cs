using CompareHare.Api.Features.Shared.Validation;
using CompareHare.Api.Features.Shared.Validation.Interfaces;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.MediatR.ValidationPipeline
{
    public class ValidatableModelRequestPreProcessor<TModel> :
        IFluentValidationRequestPreProcessor<IValidatableRequest<TModel>>,
        ICustomValidationRequestPreProcessor<IValidatableRequest<TModel>>
    {
        private readonly Lazy<IEnumerable<ICustomValidator<TModel>>> _customValidators;
        private readonly Lazy<IEnumerable<IValidator<TModel>>> _validators;

        public ValidatableModelRequestPreProcessor(
            Lazy<IEnumerable<IValidator<TModel>>> validators,
            Lazy<IEnumerable<ICustomValidator<TModel>>> customValidators)
        {
            _validators = validators;
            _customValidators = customValidators;
        }

        async Task<IActionResult> ICustomValidationRequestPreProcessor<IValidatableRequest<TModel>>.Process(IValidatableRequest<TModel> request)
        {
            var customValidationFailures = new List<CustomValidationFailure>();

            foreach (var cv in _customValidators.Value) customValidationFailures.AddRange(await cv.ValidateAsync(request.Model));

#pragma warning disable CA1860 // Avoid using 'Enumerable.Any()' extension method
#pragma warning disable CS8603 // Possible null reference return.
            return customValidationFailures.Any()
                       ? new BadRequestObjectResult(
                           customValidationFailures
                              .GroupBy(vf => vf.PropertyName, vf => vf.ErrorMessage)
                              .ToDictionary(pe => pe.Key, pe => pe.ToArray()))
                       : null;

        }

        async Task<IActionResult> IFluentValidationRequestPreProcessor<IValidatableRequest<TModel>>.Process(IValidatableRequest<TModel> request)
        {
            var validationFailures = new List<ValidationFailure>();

            foreach (var v in _validators.Value) validationFailures.AddRange((await v.ValidateAsync(request.Model)).Errors);

            return validationFailures.Any()
                       ? new BadRequestObjectResult(
                           validationFailures
                              .GroupBy(vf => vf.PropertyName, vf => vf.ErrorMessage)
                              .ToDictionary(pe => pe.Key, pe => pe.ToArray()))
                       : null;
#pragma warning restore CS8603 // Possible null reference return.
#pragma warning restore CA1860 // Avoid using 'Enumerable.Any()' extension method
        }
    }
}
