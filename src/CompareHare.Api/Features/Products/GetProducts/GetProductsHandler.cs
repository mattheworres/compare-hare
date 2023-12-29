using CompareHare.Api.MediatR;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Products.GetProducts
{
    public class GetProductsHandler : ApiRequestHandlerBase, IRequestHandler<GetProductsMessage, IActionResult>
    {
        public async Task<IActionResult> Handle(GetProductsMessage request, CancellationToken cancellationToken)
        {
            await Task.Delay(1000);

            return Ok("Hey, we waited 1 second, and now here we are!");
        }
    }
}
