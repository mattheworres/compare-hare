using AutoMapper;
using CompareHare.Api.Features.Prices.Services.Interfaces;
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
        private readonly IPriceHelper _priceHelper;

        public AddManualPriceHandler(CompareHareDbContext dbContext, IMapper mapper, IPriceHelper priceHelper)
        {
            _priceHelper = priceHelper;
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
                x.PriceDate < model.PriceDate)
                .OrderByDescending(x => x.PriceDate)
                .FirstOrDefaultAsync();

            var closestNextPriceHistory = await _dbContext.ProductRetailerPriceHistories.Where(x =>
                x.TrackedProductRetailerId == model.TrackedProductRetailerId &&
                x.PriceDate > model.PriceDate)
                .OrderBy(x => x.PriceDate)
                .FirstOrDefaultAsync();

            var newPriceHistory = _mapper.Map<ProductRetailerPriceHistory>(model);
            _mapper.Map(trackedProductRetailer, newPriceHistory);
            await _dbContext.ProductRetailerPriceHistories.AddAsync(newPriceHistory);

            // If there's a previous price, use it to calc our diff fields on the new ph
            if (closestPreviousPriceHistory != null
                && newPriceHistory.Price.HasValue
                && closestPreviousPriceHistory.Price.HasValue)
            {
                newPriceHistory.AmountChange = _priceHelper.CalculatePriceChange(newPriceHistory.Price.Value, closestPreviousPriceHistory.Price.Value);
                newPriceHistory.PercentChange = _priceHelper.CalculatePriceChangePercentage(newPriceHistory.Price.Value, closestPreviousPriceHistory.Price.Value);
            }

            // If there's a next price, we need to update its diff fields using the new ph's value
            if (closestNextPriceHistory != null
                && closestNextPriceHistory.Price.HasValue
                && newPriceHistory.Price.HasValue)
            {
                var closestNextPrice = await _dbContext.ProductRetailerPrices.FirstAsync(x => x.ProductRetailerPriceHistoryId == closestNextPriceHistory.Id);
                var amountChange = _priceHelper.CalculatePriceChange(closestNextPriceHistory.Price.Value, newPriceHistory.Price.Value);
                var percentChange = _priceHelper.CalculatePriceChangePercentage(closestNextPriceHistory.Price.Value, newPriceHistory.Price.Value);
                closestNextPriceHistory.AmountChange = amountChange;
                closestNextPriceHistory.PercentChange = percentChange;
                closestNextPrice.AmountChange = amountChange;
                closestNextPrice.PercentChange = percentChange;
            }
            else
            { // if there's no next price, we know we need to update the existing price linked to match this price history (it just became the leader)
                var existingPrice = await _dbContext.ProductRetailerPrices.FirstOrDefaultAsync(x => x.TrackedProductRetailerId == trackedProductRetailer.Id);

                if (existingPrice == null)
                { // Create a new one
                    var newPrice = _mapper.Map<ProductRetailerPrice>(newPriceHistory);
                    await _dbContext.ProductRetailerPrices.AddAsync(newPrice);
                }
                else
                {
                    _mapper.Map(newPriceHistory, existingPrice);
                }
            }

            await _dbContext.SaveChangesAsync();

            return Ok();
        }
    }
}
