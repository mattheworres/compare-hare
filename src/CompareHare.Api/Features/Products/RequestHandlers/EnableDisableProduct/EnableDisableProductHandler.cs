using CompareHare.Api.MediatR;
using CompareHare.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CompareHare.Api.Features.Products.RequestHandlers.EnableDisableProduct
{
    public class EnableDisableProductHandler : ApiRequestHandlerBase, IRequestHandler<EnableDisableProductMessage, IActionResult>
    {
        private readonly CompareHareDbContext _dbContext;
        public EnableDisableProductHandler(CompareHareDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Handle(EnableDisableProductMessage message, CancellationToken cancellationToken)
        {
            var model = message.Model;
            var product = await _dbContext.TrackedProducts.FindAsync(model.TrackedProductId);

            if (product == null) {
                Log.Logger.Error("EnableDisableProductHandler: product null");
                return BadRequest();
            }

            product.Enabled = model.Enabled;

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
