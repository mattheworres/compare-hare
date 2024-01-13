using CompareHare.Api.Features.Products.Models;
using CompareHare.Api.Features.Shared.Validation.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Products.RequestHandlers.GetProductCurrent
{
    public class GetProductCurrentMessage : IRequest<IActionResult>, IValidatableRequest<GetProductCurrentDisplayModel>
    {
        public GetProductCurrentMessage(GetProductCurrentDisplayModel model)
        {
            Model = model;
        }

        public GetProductCurrentDisplayModel Model { get; }
    }
}