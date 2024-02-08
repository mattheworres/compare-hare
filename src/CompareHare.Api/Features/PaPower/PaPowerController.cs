using CompareHare.Api.Controllers;
using CompareHare.Api.Features.PaPower.RequestHandlers.DistributorsList;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.PaPower
{
    [Route("api/paPower")]
    public class PaPowerController : SpaApiController
    {
        private readonly IMediator _mediator;

        public PaPowerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("distributors/list/{zipCode}"), HttpGet]
        public async Task<IActionResult> PopulateOffers(string zipCode)
        {
            var model = new DistributorsListModel(zipCode);
            return await _mediator.Send(new DistributorsListMessage(model));
        }
    }
}
