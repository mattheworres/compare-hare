using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Products.RequestHandlers.GetCreateProduct
{
    public class GetCreateProductMessage : IRequest<IActionResult> {}
}