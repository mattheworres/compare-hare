using System.Threading.Tasks;
using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Api.Features.Shared.Validation;
using CompareHare.Api.Features.Shared.Validation.Interfaces;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Authentication.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Api.Features.Alerts.RequestHandlers.DeleteAlert
{
  public class DeleteAlertCustomValidator : ICustomValidator<DeleteAlertModel>
  {
    private readonly CompareHareDbContext _dbContext;
    private readonly ICurrentUserProvider _currentUserProvider;

    public DeleteAlertCustomValidator(CompareHareDbContext dbContext, ICurrentUserProvider currentUserProvider)
    {
      _dbContext = dbContext;
      _currentUserProvider = currentUserProvider;
    }

    public async Task<CustomValidationFailures> ValidateAsync(DeleteAlertModel model)
    {
      var customErrors = new CustomValidationFailures();

      var currentUserId = await _currentUserProvider.GetUserIdAsync();

      var alertExistsAndIsOwnedByUser = await _dbContext.Alerts.AnyAsync(x => x.Id == model.AlertId && x.UserId == currentUserId);

      if (!alertExistsAndIsOwnedByUser) {
          customErrors.Add("page", "Alert not found");
      }

      return customErrors;
    }
  }
}
