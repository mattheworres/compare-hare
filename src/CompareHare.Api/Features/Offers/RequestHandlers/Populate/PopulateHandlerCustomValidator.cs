using CompareHare.Api.Features.Shared.Validation;
using CompareHare.Api.Features.Shared.Validation.Interfaces;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Authentication.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Api.Features.Offers.RequestHandlers.Populate
{
    public class PopulateHandlerCustomValidator : ICustomValidator<PopulateModel>
    {
        private readonly CompareHareDbContext _dbContext;
        private readonly ICurrentUserProvider _currentUserProvider;

        public PopulateHandlerCustomValidator(CompareHareDbContext dbContext, ICurrentUserProvider currentUserProvider) {
            _dbContext = dbContext;
            _currentUserProvider = currentUserProvider;
        }

        public async Task<CustomValidationFailures> ValidateAsync(PopulateModel model)
        {

            var failures = new CustomValidationFailures();
            var currentUserId = _currentUserProvider.GetUserIdSync();
            var alertIsOwnedByUser = await _dbContext.Alerts.AnyAsync(x => x.Id == model.AlertId && x.UserId == currentUserId);

            if (!alertIsOwnedByUser) {
                failures.Add("userId", "User does not own alert");
            }

            return failures;
        }
    }
}
