using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Api.Features.Shared.Validation.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Alerts.RequestHandlers.GetAlert
{
    public class GetAlertMessage : IRequest<IActionResult>, IValidatableRequest<GetAlertModel>
    {
        public GetAlertMessage(GetAlertModel model)
        {
            Model = model;
        }

        public GetAlertModel Model { get; }
    }
}
