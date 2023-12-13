using System.Threading;
using System.Threading.Tasks;
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
        public async Task<IActionResult> Handle(GetCreateProductMessage request, CancellationToken cancellationToken)
        {
            var retailers = EnumApiHelper.GetEnumSelectListOptions(typeof(ProductRetailer));

            return Ok(new GetCreateProductModel(retailers));
        }
    }
}
