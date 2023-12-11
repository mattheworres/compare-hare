using System.Threading;
using System.Threading.Tasks;
using CompareHare.Api.BackgroundJobs.Interfaces;
using CompareHare.Api.Features.Prices.Services.Interfaces;

namespace CompareHare.Api.BackgroundJobs
{
    public class PriceLoaderJob : IPriceLoaderJob
    {
        private readonly IPriceScraperRunner _priceScraperRunner;

        public PriceLoaderJob(IPriceScraperRunner priceScraperRunner)
        {
            _priceScraperRunner = priceScraperRunner;
        }

        public async Task Run(CancellationToken ct)
        {
            await _priceScraperRunner.LoadAllPrices(ct);
        }
    }
}
