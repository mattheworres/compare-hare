using System.Reflection;
using AutoMapper;
using CompareHare.Api.Features.Prices.Services.Interfaces;
using CompareHare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CompareHare.Api.Features.Prices.Services
{
    public class PricePersister : IPricePersister
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public PricePersister(IConfiguration configuration, IMapper mapper)
        {
            _mapper = mapper;
            _configuration = configuration;
        }

        public void PersistNewPrice(ProductRetailerPrice price, int? currentPriceId)
        {
            var dbOptionsBuilder = GetDbContextOptionsBuilder();
            var retailerName = price.ProductRetailer.ToString();

            Log.Logger.Information("Persisting price for product {0} for retailer {1}", price.TrackedProductId, retailerName);

            // Note, I hit a *lot* of method not found errors when running EF SaveChangesAsync
            using (var context = new CompareHareDbContext(dbOptionsBuilder.Options))
            {
                ProductRetailerPrice priceToUpdate;
                // Load current Price + Price History if there is one
                if (currentPriceId.HasValue && price.Price.HasValue) {
                    priceToUpdate = context.ProductRetailerPrices.Find(currentPriceId.Value);
                    var oldPriceHistory = context.ProductRetailerPriceHistories.Find(priceToUpdate.ProductRetailerPriceHistoryId);

                    // Calculate change values for new price
                    priceToUpdate.AmountChange = price.Price.Value - oldPriceHistory.Price.Value;
                    priceToUpdate.PercentChange = -(1-(price.Price.Value / oldPriceHistory.Price.Value));
                    Log.Logger.Information("Price change of {0} ({1}% change) for retailer {2}", priceToUpdate.AmountChange.Value, priceToUpdate.PercentChange.Value * 100, retailerName);
                    // Update existing price: price, change fields, PH Id
                    _mapper.Map(price, priceToUpdate);
                } else {
                    priceToUpdate = price;
                    Log.Logger.Information("We got a new price here for {0}...", retailerName);

                    context.ProductRetailerPrices.Add(priceToUpdate);
                }

                // Create new PH thru mapping, persist it
                var newPriceHistory = _mapper.Map<ProductRetailerPriceHistory>(priceToUpdate);
                // Update existing price: PH Id
                priceToUpdate.ProductRetailerPriceHistory = newPriceHistory;
                context.ProductRetailerPriceHistories.Add(newPriceHistory);
                Log.Logger.Information("New price history #{0} for {1} created", newPriceHistory.Id, retailerName);

                // Persist price creation/changes
                context.SaveChanges();

                context.Dispose();

                Log.Logger.Information("Price persisted");
            }
        }

        private DbContextOptionsBuilder<CompareHareDbContext> GetDbContextOptionsBuilder()
        {
            var builder = new DbContextOptionsBuilder<CompareHareDbContext>();

            return builder.UseMySql(
                _configuration.GetConnectionString("CompareHareDbContext"),
                mySqlServerBuilder => mySqlServerBuilder
                    .MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
        }
    }
}
