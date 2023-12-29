using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Products.GetProducts
{
    public class GetProductsMessage : IRequest<IActionResult> {}
}
