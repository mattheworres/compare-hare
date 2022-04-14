using System.Threading;
using System.Threading.Tasks;
using CompareHare.Api.MediatR;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Authentication.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CompareHare.Api.Features.Prices.CheckPriceDate
{
    public class CheckPriceDateHandler : ApiRequestHandlerBase, IRequestHandler<CheckPriceDateMessage, IActionResult>
    {
        private readonly CompareHareDbContext _dbContext;
        private readonly ICurrentUserProvider _currentUserProvider;

        public CheckPriceDateHandler(CompareHareDbContext dbContext, ICurrentUserProvider currentUserProvider)
        {
            _currentUserProvider = currentUserProvider;
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Handle(CheckPriceDateMessage message, CancellationToken cancellationToken)
        {
            var model = message.Model;

            var trackedProductRetailer = await _dbContext.TrackedProductRetailers.FirstOrDefaultAsync(x => x.Id == model.TrackedProductRetailerId);

            if (trackedProductRetailer == null) {
                return Ok(false);
            }

            var currentUserId = await _currentUserProvider.GetUserIdAsync();
            var trackedProduct = await _dbContext.TrackedProducts.FirstOrDefaultAsync(x => 
                x.Id == trackedProductRetailer.TrackedProductId &&
                x.UserId == currentUserId);

            if (trackedProduct == null) {
                return Ok(false);
            }

            var datePart = model.Date.Date;
            var hasExistingPriceOnDate = await _dbContext.ProductRetailerPriceHistories.AnyAsync(x =>
                x.TrackedProductId == trackedProduct.Id &&
                x.TrackedProductRetailerId == trackedProductRetailer.Id &&
                x.PriceDate.Year == datePart.Year &&
                x.PriceDate.Month == datePart.Month &&
                x.PriceDate.Day == datePart.Day);

            return Ok(!hasExistingPriceOnDate);
        }
    }
}