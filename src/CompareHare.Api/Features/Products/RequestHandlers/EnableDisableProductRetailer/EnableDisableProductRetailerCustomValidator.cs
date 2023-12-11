using System.Linq;
using System.Threading.Tasks;
using CompareHare.Api.Features.Products.Models;
using CompareHare.Api.Features.Shared.Validation;
using CompareHare.Api.Features.Shared.Validation.Interfaces;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Authentication.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CompareHare.Api.Features.Products.RequestHandlers.EnableDisableProductRetailer
{
    public class EnableDisableProductRetailerValidation : ICustomValidator<EnableDisableProductRetailerModel>
    {
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly CompareHareDbContext _dbContext;
        public EnableDisableProductRetailerValidation(ICurrentUserProvider currentUserProvider, CompareHareDbContext dbContext)
        {
            _dbContext = dbContext;
            _currentUserProvider = currentUserProvider;
        }

        public async Task<CustomValidationFailures> ValidateAsync(EnableDisableProductRetailerModel model)
        {
            var customErrors = new CustomValidationFailures();
            var currentUserId = await _currentUserProvider.GetUserIdAsync();
            Log.Logger.Information("ID {0} user {1}", model.TrackedProductRetailerId, currentUserId);
            var retailer = await _dbContext.TrackedProductRetailers
                .Where(x => x.Id == model.TrackedProductRetailerId)
                .Include(x => x.TrackedProduct)
                .FirstOrDefaultAsync();

            var hasRetailer = retailer != null;
            var productBelongsToUser = hasRetailer
                ? await _dbContext.TrackedProducts.AnyAsync(x => x.Id == retailer.TrackedProductId && x.UserId == currentUserId)
                : false;

            if (!hasRetailer || !productBelongsToUser) {
                customErrors.Add("page", "Retailer not found");
            }

            return customErrors;
        }
    }
}