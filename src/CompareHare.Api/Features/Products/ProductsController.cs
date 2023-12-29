using CompareHare.Api.Controllers;
using CompareHare.Api.Features.Products.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Products
{
    public class ProductsController : SpaApiController
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
            => await _mediator.Send(new GetProductsMessage());
    }

}
