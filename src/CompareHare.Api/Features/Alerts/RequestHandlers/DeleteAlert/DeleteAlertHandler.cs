using System.Threading;

using CompareHare.Api.MediatR;
using CompareHare.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Api.Features.Alerts.RequestHandlers.DeleteAlert
{
  public class DeleteAlertHandler : ApiRequestHandlerBase, IRequestHandler<DeleteAlertMessage, IActionResult>
  {
    private readonly CompareHareDbContext _dbContext;

    public DeleteAlertHandler(CompareHareDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public async Task<IActionResult> Handle(DeleteAlertMessage request, CancellationToken cancellationToken)
    {
      var alert = await _dbContext.Alerts.FindAsync(request.Model.AlertId);
      var alertId = alert.Id;
      var suiId = alert.StateUtilityIndexId;

      if (alert.AlertMatch != null) {
          _dbContext.AlertMatchUtilityPriceHistories.RemoveRange(alert.AlertMatch.UtilityPriceHistories);
          _dbContext.AlertMatches.Remove(alert.AlertMatch);
      }

      _dbContext.Alerts.Remove(alert);

      if (!await _dbContext.Alerts.AnyAsync(x => x.Id != alertId && x.StateUtilityIndexId == suiId)) {
          var sui = await _dbContext.StateUtilityIndices.FindAsync(suiId);
          sui.Active = false;
          sui.LastUpdatedHash = string.Empty;

          await _dbContext.SaveChangesAsync();
      }

      await _dbContext.SaveChangesAsync();

      return Ok();
    }
  }
}
