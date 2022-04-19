using System.Threading.Tasks;
using CompareHare.Api.Features.Products.Models;
using CompareHare.Api.Features.Shared.Validation;
using CompareHare.Api.Features.Shared.Validation.Interfaces;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Authentication.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Api.Features.Products.RequestHandlers.EnableDisableProduct
{
    public class EnableDisableProductValidation : ICustomValidator<EnableDisableProductModel>
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly CompareHareDbContext _dbContext;
        public EnableDisableProductValidation(ICurrentUserProvider currentUserProvider, CompareHareDbContext dbContext)
        {
            _dbContext = dbContext;
            _currentUserProvider = currentUserProvider;
        }

        public async Task<CustomValidationFailures> ValidateAsync(EnableDisableProductModel model)
        {
            var customErrors = new CustomValidationFailures();
            var currentUserId = await _currentUserProvider.GetUserIdAsync();

            if (!await _dbContext.TrackedProducts.AnyAsync(x => x.Id == model.TrackedProductId && x.UserId == currentUserId)) {
                customErrors.Add("page", "Product not found");
            }

            return customErrors;
        }
    }
}