using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Shared.Validation.Interfaces
{
    public interface IValidatableRequest<out TModel> : IRequest<IActionResult>
    {
        TModel Model { get; }
    }

    public interface IValidatableRequest : IRequest<IActionResult> { }
}
