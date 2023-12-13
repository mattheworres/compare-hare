using CompareHare.Api.Features.Products.Models;
using CompareHare.Api.Features.Shared.Validation.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Products.RequestHandlers.EnableDisableProduct
{
    public class EnableDisableProductMessage : IRequest<IActionResult>, IValidatableRequest<EnableDisableProductModel>
    {
        public EnableDisableProductModel Model { get; }

        public EnableDisableProductMessage(EnableDisableProductModel model)
        {
            Model = model;
        }
    }
}