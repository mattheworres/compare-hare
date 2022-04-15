using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CompareHare.Api.MediatR;
using CompareHare.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Api.Features.Prices.AddManualPrice
{
    public class AddManualPriceHandler : ApiRequestHandlerBase, IRequestHandler<AddManualPriceMessage, IActionResult>
    {
        private readonly CompareHareDbContext _dbContext;
        private readonly IMapper _mapper;

        public AddManualPriceHandler(CompareHareDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IActionResult> Handle(AddManualPriceMessage message, CancellationToken cancellationToken)
        {
            var model = message.Model;
            //get the closest price history newer than this one (query abs on datetime)
            var trackedProductRetailer = await _dbContext.TrackedProductRetailers.SingleAsync(x => x.Id == model.TrackedProductRetailerId);
            var trackedProduct = await _dbContext.TrackedProducts.SingleAsync(x => x.Id == trackedProductRetailer.TrackedProductId);

            var closestPreviousPriceHistory = await _dbContext.ProductRetailerPriceHistories.Where(x =>
                x.TrackedProductRetailerId == model.TrackedProductRetailerId &&
                x.CreatedDate < model.PriceDate)
                .OrderByDescending(x => x.CreatedDate)
                .FirstOrDefaultAsync();

            var closestNextPriceHistory = await _dbContext.ProductRetailerPriceHistories.Where(x =>
                x.TrackedProductRetailerId == model.TrackedProductRetailerId &&
                x.CreatedDate > model.PriceDate)
                .OrderBy(x => x.CreatedDate)
                .FirstOrDefaultAsync();

            var newPriceHistory = _mapper.Map<ProductRetailerPriceHistory>(model);
            _mapper.Map(trackedProductRetailer, newPriceHistory);
            await _dbContext.ProductRetailerPriceHistories.AddAsync(newPriceHistory);
            await _dbContext.SaveChangesAsync();

            // If there's a previous price, use it to calc our diff fields on the new ph
            if (closestPreviousPriceHistory != null) {
                newPriceHistory.AmountChange = newPriceHistory.Price.Value - closestPreviousPriceHistory.Price.Value;
                newPriceHistory.PercentChange = -(1-(newPriceHistory.Price.Value / closestPreviousPriceHistory.Price.Value));
            }

            // If there's a next price, we need to update its diff fields using the new ph's value
            if (closestNextPriceHistory != null) {
                closestNextPriceHistory.AmountChange = closestNextPriceHistory.Price.Value - newPriceHistory.Price.Value;
                closestNextPriceHistory.PercentChange = -(1-(closestNextPriceHistory.Price.Value / newPriceHistory.Price.Value));
            } else { // if there's no next price, we know we need to update the existing price linked to match this price history (it just became the leader)
                var existingPrice = await _dbContext.ProductRetailerPrices.FirstOrDefaultAsync(x => x.TrackedProductRetailerId == trackedProductRetailer.Id);

                if (existingPrice == null) { // Create a new one
                    var newPrice = _mapper.Map<ProductRetailerPrice>(newPriceHistory);
                    await _dbContext.ProductRetailerPrices.AddAsync(newPrice);
                } else {
                    _mapper.Map(newPriceHistory, existingPrice);
                }
            }

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}