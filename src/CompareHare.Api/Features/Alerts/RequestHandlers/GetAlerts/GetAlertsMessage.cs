using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Alerts.RequestHandlers.GetAlerts
{
    public class GetAlertsMessage : IRequest<IActionResult> {}
}
