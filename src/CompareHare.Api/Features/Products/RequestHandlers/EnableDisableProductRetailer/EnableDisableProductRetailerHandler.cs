using System.Threading;
using System.Threading.Tasks;
using CompareHare.Api.MediatR;
using CompareHare.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Products.RequestHandlers.EnableDisableProductRetailer
{
    public class EnableDisableProductRetailerHandler : ApiRequestHandlerBase, IRequestHandler<EnableDisableProductRetailerMessage, IActionResult>
    {
        private readonly CompareHareDbContext _dbContext;
        public EnableDisableProductRetailerHandler(CompareHareDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Handle(EnableDisableProductRetailerMessage message, CancellationToken cancellationToken)
        {
            var model = message.Model;
            var retailer = await _dbContext.TrackedProductRetailers.FindAsync(model.TrackedProductRetailerId);

            retailer.Enabled = model.Enabled;

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}