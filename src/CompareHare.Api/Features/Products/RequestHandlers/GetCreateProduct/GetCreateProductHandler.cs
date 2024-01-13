using CompareHare.Api.Features.Products.Models;
using CompareHare.Api.Features.Shared.Services;
using CompareHare.Api.MediatR;
using CompareHare.Domain.Entities.Constants;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Products.RequestHandlers.GetCreateProduct
{
    public class GetCreateProductHandler : ApiRequestHandlerBase, IRequestHandler<GetCreateProductMessage, IActionResult>
    {
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IActionResult> Handle(GetCreateProductMessage request, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var retailers = EnumApiHelper.GetEnumSelectListOptions(typeof(ProductRetailer));

            return Ok(new GetCreateProductModel(retailers));
        }
    }
}
