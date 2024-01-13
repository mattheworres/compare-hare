using System.Reflection;
using Autofac;
using CompareHare.Domain.Entities;
using CompareHare.Api.Features.Offers.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Serilog;

namespace CompareHare.Api.Features.Offers.Services
{
    public class OfferPersister : IOfferPersister
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public OfferPersister(
            IConfiguration configuration,
            IMapper mapper)
        {
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task PersistNewOffers(IEnumerable<UtilityPrice> utilityPrices, int utilityIndexId, string offerHash)
        {
            var container = new ContainerBuilder();
            var dbOptionsBuilder = GetDbContextOptionsBuilder();

            Log.Logger.Information("OfferPersister: Persisting offers...");

            using (var context = new CompareHareDbContext(dbOptionsBuilder.Options))
            {
                Log.Logger.Information("OfferPersister: Deleting existing offers");
                var existingOffers = await context.UtilityPrices.Where(x => x.StateUtilityIndexId == utilityIndexId).ToListAsync();
                context.RemoveRange(existingOffers);

                //Mattnote: looks like I struggled with the async nature of Hangfire jobs
                //re: EF contexts. I think the options builder is the way I solved it.
                //need to confirm this is the case when I return to developing utility scraping
                //TODO: Remove once done figuring this shit out
                var existingHistoricals = await context.UtilityPriceHistories.Where(x => x.StateUtilityIndexId == utilityIndexId).ToListAsync();
                context.RemoveRange(existingHistoricals);

                Log.Logger.Information("OfferPersister: Mapping to histories");

                //var historicalPrices = _mapper.Map<IEnumerable<UtilityPriceHistory>>(utilityPrices);

                //Hefty, but cheapest way to get link between them for assessors later
                foreach (var utilityPrice in utilityPrices)
                {
                    var historical = _mapper.Map<UtilityPriceHistory>(utilityPrice);
                    utilityPrice.UtilityPriceHistory = historical;

                    await context.UtilityPriceHistories.AddAsync(historical);
                    await context.UtilityPrices.AddAsync(utilityPrice);
                }
                // await context.UtilityPrices.AddRangeAsync(utilityPrices);
                // await context.UtilityPriceHistories.AddRangeAsync(historicalPrices);

                Log.Logger.Information("OfferPersister: Save new additions");

                await context.SaveChangesAsync();

                Log.Logger.Information("OfferPersister: Update the index");

                var index = await context.StateUtilityIndices.FindAsync(utilityIndexId);

                if (index != null) {
                    index.LastUpdatedHash = offerHash;
                }

                Log.Logger.Information("OfferPersister: Save one more bit of changes");

                await context.SaveChangesAsync();

                context.Dispose();
            }
        }

        private DbContextOptionsBuilder<CompareHareDbContext> GetDbContextOptionsBuilder()
        {
            var builder = new DbContextOptionsBuilder<CompareHareDbContext>();
            var connectionString = _configuration.GetConnectionString("CompareHareDbContext");
            // Long term I'd prefer not to hard code version of DB, rather rely on conn string.
            // If this doesn't work well, we may just stuff that in the appsettings as well.
            var version = ServerVersion.AutoDetect(connectionString);

            return builder.UseMySql(connectionString,
                version,
                mySqlServerBuilder => mySqlServerBuilder
                    .MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
        }
    }
}
