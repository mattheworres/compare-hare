using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.MediatR.ValidationPipeline
{
    public interface ICustomValidationRequestPreProcessor<in TRequest>
    {
        Task<IActionResult> Process(TRequest request);
    }
}
