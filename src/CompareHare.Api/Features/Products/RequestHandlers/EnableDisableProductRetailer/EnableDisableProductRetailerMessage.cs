using CompareHare.Api.Features.Products.Models;
using CompareHare.Api.Features.Shared.Validation.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Products.RequestHandlers.EnableDisableProductRetailer
{
    public class EnableDisableProductRetailerMessage : IRequest<IActionResult>, IValidatableRequest<EnableDisableProductRetailerModel>
    {
        public EnableDisableProductRetailerModel Model { get; }

        public EnableDisableProductRetailerMessage(EnableDisableProductRetailerModel model)
        {
            Model = model;
        }
    }
}