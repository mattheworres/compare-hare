
using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Api.Features.Shared.Validation;
using CompareHare.Api.Features.Shared.Validation.Interfaces;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Authentication.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Api.Features.Alerts.RequestHandlers.GetEditAlert
{
  public class GetEditAlertCustomValidator : ICustomValidator<EditAlertModel>
  {
    private readonly CompareHareDbContext _dbContext;
    private readonly ICurrentUserProvider _currentUserProvider;

    public GetEditAlertCustomValidator(CompareHareDbContext dbContext, ICurrentUserProvider currentUserProvider)
    {
      _dbContext = dbContext;
      _currentUserProvider = currentUserProvider;
    }

    public async Task<CustomValidationFailures> ValidateAsync(EditAlertModel model)
    {
      var customErrors = new CustomValidationFailures();

      var currentUserId = _currentUserProvider.GetUserIdSync();

      var alertExistsAndIsOwnedByUser = await _dbContext.Alerts.AnyAsync(x => x.Id == model.Id && x.UserId == currentUserId);

      if (!alertExistsAndIsOwnedByUser)
      {
        customErrors.Add("page", "Alert not found");
      }

      return customErrors;
    }
  }
}
