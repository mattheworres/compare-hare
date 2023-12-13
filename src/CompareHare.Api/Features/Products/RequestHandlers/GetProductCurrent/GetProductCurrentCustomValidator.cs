using System.Threading.Tasks;
using CompareHare.Api.Features.Products.Models;
using CompareHare.Api.Features.Shared.Validation;
using CompareHare.Api.Features.Shared.Validation.Interfaces;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Authentication.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Api.Features.Products.RequestHandlers.GetProductCurrent
{
    public class GetProductCurrentCustomValidator : ICustomValidator<GetProductCurrentDisplayModel>
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly CompareHareDbContext _dbContext;

        public GetProductCurrentCustomValidator(ICurrentUserProvider currentUserProvider, CompareHareDbContext dbContext)
        {
            _currentUserProvider = currentUserProvider;
            _dbContext = dbContext;
        }

        public async Task<CustomValidationFailures> ValidateAsync(GetProductCurrentDisplayModel model)
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