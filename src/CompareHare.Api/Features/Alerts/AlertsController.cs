using System.Threading.Tasks;
using CompareHare.Api.Controllers;
using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Api.Features.Alerts.RequestHandlers.CreateAlert;
using CompareHare.Api.Features.Alerts.RequestHandlers.DeleteAlert;
using CompareHare.Api.Features.Alerts.RequestHandlers.GetAlert;
using CompareHare.Api.Features.Alerts.RequestHandlers.GetAlerts;
using CompareHare.Api.Features.Alerts.RequestHandlers.GetEditAlert;
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

        [HttpGet("list")]
        public async Task<IActionResult> GetAlertsList()
            => await _mediator.Send(new GetAlertsMessage());

        [HttpGet("{alertId:int}")]
        public async Task<IActionResult> GetAlert(int alertId)
            => await _mediator.Send(new GetAlertMessage(new GetAlertModel(alertId)));

        [HttpPost]
        public async Task<IActionResult> CreateAlert([FromBody] CreateAlertModel model)
            => await _mediator.Send(new CreateAlertMessage(model));

        [HttpDelete("{alertId:int}")]
        public async Task<IActionResult> DeleteAlert(int alertId)
            => await _mediator.Send(new DeleteAlertMessage(new DeleteAlertModel(alertId)));

        // [HttpGet("{alertId:int}/edit")]
        // public async Task<IActionResult> GetAlertEdit(int alertId)
        //     => await _mediator.Send(new GetEditAlertMessage(new EditAlertModel(alertId)));
    }
}
