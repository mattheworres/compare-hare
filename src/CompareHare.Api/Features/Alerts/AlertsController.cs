using System.Threading.Tasks;
using CompareHare.Api.Controllers;
using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Api.Features.Alerts.RequestHandlers.CreateAlert;
using CompareHare.Api.Features.Alerts.RequestHandlers.GetAlerts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Alerts
{
    [Route("api/alerts")]
    public class AlertsController : SpaApiController
    {
        private readonly IMediator _mediator;

        public AlertsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAlertsList() {
            return await _mediator.Send(new GetAlertsMessage());
        }

        [HttpPost]
        public async Task<IActionResult> CreateAlert([FromBody] CreateAlertModel model)
        {
            return await _mediator.Send(new CreateAlertMessage(model));
        }
    }
}
