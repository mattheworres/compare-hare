using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.MediatR.ValidationPipeline
{
    public class ValidatableRequestPreProcessorBehavior<TRequest> : IPipelineBehavior<TRequest, IActionResult>
    {
        private readonly IEnumerable<ICustomValidationRequestPreProcessor<TRequest>> _customValidationPreProcessors;
        private readonly IEnumerable<IFluentValidationRequestPreProcessor<TRequest>> _validationPreProcessors;

        public ValidatableRequestPreProcessorBehavior(
            IEnumerable<IFluentValidationRequestPreProcessor<TRequest>> validationPreProcessors,
            IEnumerable<ICustomValidationRequestPreProcessor<TRequest>> customValidationPreProcessors)
        {
            _validationPreProcessors = validationPreProcessors;
            _customValidationPreProcessors = customValidationPreProcessors;
        }

        public async Task<IActionResult> Handle(TRequest request, RequestHandlerDelegate<IActionResult> next, CancellationToken cancellationToken)
        {
            foreach (var validationPreProcessor in _validationPreProcessors)
            {
                var result = await validationPreProcessor.Process(request);
                if (result == null) continue;

                return result;
            }

            foreach (var customValidationRequestPreProcessor in _customValidationPreProcessors)
            {
                var result = await customValidationRequestPreProcessor.Process(request);
                if (result == null) continue;

                return result;
            }

            return await next();
        }
    }
}
