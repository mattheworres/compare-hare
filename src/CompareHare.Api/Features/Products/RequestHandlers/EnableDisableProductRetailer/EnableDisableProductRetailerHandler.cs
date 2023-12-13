using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CompareHare.Api.MediatR;
using CompareHare.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

            // Regardless of enable/disable, wipe any exceptions associated with this retailer
            if (await _dbContext.ProductPriceScrapingExceptions
                .AnyAsync(x => x.TrackedProductId == retailer.TrackedProductId && x.TrackedProductRetailerId == retailer.Id)) {
                var exceptionsToRemove = await _dbContext.ProductPriceScrapingExceptions
                    .Where(x => x.TrackedProductId == retailer.TrackedProductId && x.TrackedProductRetailerId == retailer.Id)
                    .ToListAsync();

                _dbContext.RemoveRange(exceptionsToRemove);
            }

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}