using CompareHare.Api.MediatR;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Authentication.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Prices.GetSummaryHistory
{
    public class GetSummaryHistoryHandler : ApiRequestHandlerBase, IRequestHandler<GetSummaryHistoryMessage, IActionResult>
    {
        private readonly CompareHareDbContext _dbContext;
        private readonly ICurrentUserProvider _currentUserProvider;

        public GetSummaryHistoryHandler(CompareHareDbContext dbContext, ICurrentUserProvider currentUserProvider)
        {
            _currentUserProvider = currentUserProvider;
            _dbContext = dbContext;
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IActionResult> Handle(GetSummaryHistoryMessage message, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            //Get all product retailers for a given product
            //Get up to <limit> of price histories per retailer
            //Place all price histories into single list, sort by date
            //Cycle through, building model in :
            //Retailers models of price values per date
            //  (if no price value this day, use same as last; if no
            //      last price, null)
            return Ok();
        }
    }
}
