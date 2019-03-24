using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Api.Features.Shared.Validation.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Alerts.RequestHandlers.CreateAlert
{
    public class CreateAlertMessage : IRequest<IActionResult>, IValidatableRequest<CreateAlertModel>
    {
        public CreateAlertMessage(CreateAlertModel model) {
            Model = model;
        }

        public CreateAlertModel Model { get; set; }
    }
}
