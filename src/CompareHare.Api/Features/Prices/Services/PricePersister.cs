using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AutoMapper;
using CompareHare.Api.Features.Prices.Services.Interfaces;
using CompareHare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CompareHare.Api.Features.Prices.Services
{
    public class PricePersister : IPricePersister
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly IPriceHelper _priceHelper;

        public PricePersister(IConfiguration configuration, IMapper mapper, IPriceHelper priceHelper)
        {
            _priceHelper = priceHelper;
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
                if (currentPriceId != null && currentPriceId.HasValue && price != null && price.Price.HasValue)
                {
                    priceToUpdate = context.ProductRetailerPrices.Find(currentPriceId.Value);
                    var oldPriceHistory = context.ProductRetailerPriceHistories.Find(priceToUpdate.ProductRetailerPriceHistoryId);

                    if (oldPriceHistory != null && oldPriceHistory.Price.HasValue) {
                        // Calculate change values for new price
                        priceToUpdate.AmountChange = _priceHelper.CalculatePriceChange(price.Price.Value, oldPriceHistory.Price.Value);
                        priceToUpdate.PercentChange = _priceHelper.CalculatePriceChangePercentage(price.Price.Value, oldPriceHistory.Price.Value);
                        Log.Logger.Information("Price change of {0} ({1}% change) for retailer {2}", priceToUpdate.AmountChange.Value, priceToUpdate.PercentChange.Value * 100, retailerName);
                        // Update existing price: price, change fields, PH Id
                        _mapper.Map(price, priceToUpdate);

                        // If the price exists and we're updating, we need to remove any exceptions tied to this retailer
                        var existingExceptions = context.ProductPriceScrapingExceptions
                            .Where(x => x.TrackedProductId == priceToUpdate.TrackedProductId && x.TrackedProductRetailerId == priceToUpdate.TrackedProductRetailerId)
                            .ToList();

                        if (existingExceptions.Any())
                        {
                            context.RemoveRange(existingExceptions);
                        }
                    }
                }
                else
                {
                    priceToUpdate = price;
                    Log.Logger.Information("We got a new price here for {0}...", retailerName);

                    context.ProductRetailerPrices.Add(priceToUpdate);
                }

                // Create new PH thru mapping, persist it
                var newPriceHistory = _mapper.Map<ProductRetailerPriceHistory>(priceToUpdate);
                // Update existing price: PH Id
                priceToUpdate.ProductRetailerPriceHistory = newPriceHistory;
                context.ProductRetailerPriceHistories.Add(newPriceHistory);
                Log.Logger.Information("New price history for {0} created", retailerName);

                // Persist price creation/changes
                context.SaveChanges();

                context.Dispose();

                Log.Logger.Information("Price persisted");
            }
        }

        public void UpdateUnchangedPrice(int currentPriceId, DateTimeOffset today)
        {
            var dbOptionsBuilder = GetDbContextOptionsBuilder();

            using (var context = new CompareHareDbContext(dbOptionsBuilder.Options))
            {
                var priceToUpdate = context.ProductRetailerPrices.Find(currentPriceId);
                priceToUpdate.PriceDate = today;

                context.SaveChanges();
            }
        }

        private DbContextOptionsBuilder<CompareHareDbContext> GetDbContextOptionsBuilder()
        {
            var builder = new DbContextOptionsBuilder<CompareHareDbContext>();
            var connectionString = _configuration.GetConnectionString("CompareHareDbContext");
            // Long term I'd prefer not to hard code version of DB, rather rely on conn string.
            // If this doesn't work well, we may just stuff that in the appsettings as well.
            var version = ServerVersion.AutoDetect(connectionString);

            return builder.UseMySql(
                connectionString,
                version,
                mySqlServerBuilder => mySqlServerBuilder
                    .MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
        }
    }
}
