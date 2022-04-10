using System.Threading.Tasks;
using CompareHare.Api.Controllers;
using CompareHare.Api.Features.Products.Models;
using CompareHare.Api.Features.Products.RequestHandlers.CreateProduct;
using CompareHare.Api.Features.Products.RequestHandlers.GetCreateProduct;
using CompareHare.Api.Features.Products.RequestHandlers.GetProductCurrent;
using CompareHare.Api.Features.Products.RequestHandlers.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Products
{
    [Route("api/products")]
    public class ProductsController : SpaApiController
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("list")]
        public async Task<IActionResult> GetProductsList()
            => await _mediator.Send(new GetProductsMessage());

        [HttpGet("current/{trackedProductId:int}")]
        public async Task<IActionResult> GetProductCurrent(int trackedProductId)
            => await _mediator.Send(new GetProductCurrentMessage(new GetProductCurrentDisplayModel(trackedProductId)));

        [HttpGet("create")]
        public async Task<IActionResult> GetProductCreate()
            => await _mediator.Send(new GetCreateProductMessage());

        [HttpPost("create")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductModel model)
            => await _mediator.Send(new CreateProductMessage(model));
    }
}
