using System.Threading.Tasks;
using CompareHare.Api.Features.Prices.Models;
using CompareHare.Api.Features.Shared.Validation;
using CompareHare.Api.Features.Shared.Validation.Interfaces;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Authentication.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Api.Features.Prices.AddManualPrice
{
    public class AddManualPriceCustomValidator : ICustomValidator<AddManualPriceModel>
    {
        private readonly CompareHareDbContext _dbContext;
        private readonly ICurrentUserProvider _currentUserProvider;

        public AddManualPriceCustomValidator(CompareHareDbContext dbContext, ICurrentUserProvider currentUserProvider)
        {
            _currentUserProvider = currentUserProvider;
            _dbContext = dbContext;
        }

        public async Task<CustomValidationFailures> ValidateAsync(AddManualPriceModel model)
        {
            var customErrors = new CustomValidationFailures();
            var currentUserId = await _currentUserProvider.GetUserIdAsync();

            var productRetailer = await _dbContext.TrackedProductRetailers
                .FirstOrDefaultAsync(x => x.Id == model.TrackedProductRetailerId);

            var product = productRetailer != null ? await _dbContext.TrackedProducts
                .FirstOrDefaultAsync(x => x.Id == productRetailer.TrackedProductId
                    && x.UserId == currentUserId)
                    : null;
            
            if (productRetailer == null || product == null) {
                customErrors.Add("page", "Product not found");
            }

            if (model.Price <= 0) {
                customErrors.Add("Price", "Price must be positive and non-zero");
            }

            var dateAlreadyExists = await _dbContext.ProductRetailerPriceHistories.AnyAsync(x =>
                x.CreatedDate.Date == model.PriceDate.Date
            );

            if (dateAlreadyExists) {
                customErrors.Add("PriceDate", "There already exists a price on that date.");
            }

            return customErrors;
        }
    }
}