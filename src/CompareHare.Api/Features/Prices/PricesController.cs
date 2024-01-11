using CompareHare.Api.Controllers;
using CompareHare.Api.Features.Prices.AddManualPrice;
using CompareHare.Api.Features.Prices.CheckPriceDate;
using CompareHare.Api.Features.Prices.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Prices
{
    [Route("api/prices")]
    public class PricesController : SpaApiController
    {
        private readonly IMediator _mediator;

        public PricesController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpGet("manual/{trackedProductRetailerId:int}/check")]
        public async Task<IActionResult> CheckPriceDate(int trackedProductRetailerId, DateTimeOffset date)
            => await _mediator.Send(new CheckPriceDateMessage(new CheckPriceDateModel(trackedProductRetailerId, date)));

        [HttpPost("manual/{trackedProductRetailerId:int}")]
        public async Task<IActionResult> AddManualPrice(int trackedProductRetailerId, [FromBody] AddManualPriceModel model)
            => await _mediator.Send(new AddManualPriceMessage(model));
    }
}
