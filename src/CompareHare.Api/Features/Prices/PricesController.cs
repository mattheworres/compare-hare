using CompareHare.Api.Controllers;
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


    }
}
