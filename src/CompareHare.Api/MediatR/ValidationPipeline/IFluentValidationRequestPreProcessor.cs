using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.MediatR.ValidationPipeline
{
    public interface IFluentValidationRequestPreProcessor<in TRequest>
    {
        Task<IActionResult> Process(TRequest request);
    }
}
