using System.Collections.Generic;
using System.Threading.Tasks;
using System.Reflection;
using Autofac;
using CompareHare.Domain.Entities;
using CompareHare.Api.Features.Offers.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using AutoMapper;

namespace CompareHare.Api.Features.Offers.Services
{
    public class OfferPersister : IOfferPersister
    {
        private readonly CompareHareDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public OfferPersister(
            CompareHareDbContext dbContext,
            IConfiguration configuration,
            IMapper mapper) {
            _dbContext = dbContext;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task PersistNewOffers(IEnumerable<UtilityPrice> utilityPrices, int utilityIndexId, string offerHash)
        {
            var container = new ContainerBuilder();
            var dbOptionsBuilder = GetDbContextOptionsBuilder();

            //Log.Logger.Information("Persisting offers...");

            using (var context = new CompareHareDbContext(dbOptionsBuilder.Options)) {
                //Log.Logger.Information("Deleting existing offers");
                var existingOffers = await context.UtilityPrices.Where(x => x.StateUtilityIndexId == utilityIndexId).ToListAsync();
                context.RemoveRange(existingOffers);

                //TODO: Remove once done figuring this shit out
                //var existingHistoricals = await context.UtilityPriceHistories.Where(x => x.StateUtilityIndexId == utilityIndexId).ToListAsync();
                //context.RemoveRange(existingHistoricals);

                //Log.Logger.Information("Mapping to histories");

                //var historicalPrices = _mapper.Map<IEnumerable<UtilityPriceHistory>>(utilityPrices);

                //Hefty, but cheapest way to get link between them for assessors later
                foreach(var utilityPrice in utilityPrices) {
                    var historical = _mapper.Map<UtilityPriceHistory>(utilityPrice);
                    utilityPrice.UtilityPriceHistory = historical;

                    await context.UtilityPriceHistories.AddAsync(historical);
                    await context.UtilityPrices.AddAsync(utilityPrice);
                }
                // await context.UtilityPrices.AddRangeAsync(utilityPrices);
                // await context.UtilityPriceHistories.AddRangeAsync(historicalPrices);

                //Log.Logger.Information("Save new additions");

                await context.SaveChangesAsync();

                //Log.Logger.Information("Update the index");

                var index = await context.StateUtilityIndices.FindAsync(utilityIndexId);
                index.LastUpdatedHash = offerHash;

                //Log.Logger.Information("Save one more bit of changes");

                await context.SaveChangesAsync();

                context.Dispose();
            }
        }

        private DbContextOptionsBuilder<CompareHareDbContext> GetDbContextOptionsBuilder() {
            var builder = new DbContextOptionsBuilder<CompareHareDbContext>();

            return builder.UseMySql(
                _configuration.GetConnectionString("CompareHareDbContext"),
                mySqlServerBuilder => mySqlServerBuilder
                    .MigrationsAssembly(Assembly.GetExecutingAssembly().FullName));
        }
    }
}
