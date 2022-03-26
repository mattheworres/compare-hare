using System.Reflection;
using System.Threading.Tasks;
using CompareHare.Api.Features.Prices.Interfaces;
using CompareHare.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace CompareHare.Api.Features.Prices
{
    public class PriceHistoryPersister : IPriceHistoryPersister
    {
        private readonly IConfiguration _configuration;

        public PriceHistoryPersister(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task PersistNewPriceHistory(ProductRetailerPriceHistory history)
        {
            var dbOptionsBuilder = GetDbContextOptionsBuilder();

            Log.Logger.Information("Persisting price history for product {0} for retailer {1}", history.TrackedProductId, history.ProductRetailer.ToString());

            using (var context = new CompareHareDbContext(dbOptionsBuilder.Options))
            {
                await context.ProductRetailerPriceHistories.AddAsync(history);
                await context.SaveChangesAsync();

                context.Dispose();

                Log.Logger.Information("History persisted");
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