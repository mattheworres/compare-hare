using System.Threading.Tasks;
using CompareHare.Api.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Offers
{
    [Route("api/[controller]")]
    public class OffersController : SpaApiController
    {
        private readonly IMediator _mediator;

        public OffersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // [Route("populate/{alertId:int}"), HttpPost]
        // public async Task<IActionResult> PopulateOffers(int alertId)
        // {
        //     var ip = this.Request.HttpContext.Connection.RemoteIpAddress;
        //     return await _mediator.Send(new LogInMessage(model, ip.ToString()));
        // }
    }
}
