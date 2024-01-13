using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Products.RequestHandlers.GetProducts
{
    public class GetProductsMessage : IRequest<IActionResult> {}
}