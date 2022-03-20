using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Api.Features.Shared.Validation.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Alerts.RequestHandlers.GetEditAlert
{
  public class GetEditAlertMessage : IRequest<IActionResult>, IValidatableRequest<EditAlertModel>
  {
    public GetEditAlertMessage(EditAlertModel model)
    {
      Model = model;
    }

    public EditAlertModel Model { get; }
  }
}
