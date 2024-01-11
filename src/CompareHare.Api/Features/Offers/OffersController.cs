using CompareHare.Api.Controllers;
using CompareHare.Api.Features.Offers.RequestHandlers.Populate;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Offers
{
    [Route("api/offers")]
    public class OffersController : SpaApiController
    {
        private readonly IMediator _mediator;

        public OffersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("populate/{alertId:int}"), HttpPost]
        public async Task<IActionResult> PopulateOffers(int alertId)
        {
            var model = new PopulateModel(alertId);
            return await _mediator.Send(new PopulateMessage(model));
        }
    }
}
