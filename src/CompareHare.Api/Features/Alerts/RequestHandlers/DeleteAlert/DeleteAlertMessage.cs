using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Api.Features.Shared.Validation.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Alerts.RequestHandlers.DeleteAlert
{
  public class DeleteAlertMessage : IRequest<IActionResult>, IValidatableRequest<DeleteAlertModel>
  {
    public DeleteAlertMessage(DeleteAlertModel model)
    {
      Model = model;
    }

    public DeleteAlertModel Model { get; }
  }
}
