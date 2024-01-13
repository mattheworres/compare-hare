using CompareHare.Api.Features.Products.Models;
using CompareHare.Api.Features.Shared.Validation.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Products.RequestHandlers.CreateProduct
{
    public class CreateProductMessage : IRequest<IActionResult>, IValidatableRequest<CreateProductModel>
    {
        public CreateProductMessage(CreateProductModel model) {
            Model = model;
        }

        public CreateProductModel Model { get; set; }
    }
}
