
using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Api.Features.Shared.Validation;
using CompareHare.Api.Features.Shared.Validation.Interfaces;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Authentication.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Api.Features.Alerts.RequestHandlers.GetAlert
{
    public class GetAlertCustomValidator : ICustomValidator<GetAlertModel>
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly CompareHareDbContext _dbContext;

        public GetAlertCustomValidator(
            ICurrentUserProvider currentUserProvider,
            CompareHareDbContext dbContext) {
            _currentUserProvider = currentUserProvider;
            _dbContext = dbContext;
        }

        public async Task<CustomValidationFailures> ValidateAsync(GetAlertModel model)
        {
            var customErrors = new CustomValidationFailures();
            var currentUserId = _currentUserProvider.GetUserIdSync();

            if (!await _dbContext.Alerts.AnyAsync(x => x.Id == model.AlertId && x.UserId == currentUserId)) {
                customErrors.Add("page", "Alert not found");
            }

            if (!await _dbContext.Alerts.AnyAsync(x => x.Id == model.AlertId)) {
                customErrors.Add("page", "Alert not found");
            }

            return customErrors;
        }
    }
}
