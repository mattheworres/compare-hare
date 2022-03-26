using System.Threading;
using System.Threading.Tasks;
using CompareHare.Api.BackgroundJobs.Interfaces;
using CompareHare.Api.Features.Prices.Interfaces;

namespace CompareHare.Api.BackgroundJobs
{
    public class PriceLoaderJob : IPriceLoaderJob
    {
        private readonly IPriceScraperRunner _priceLoaderRunner;

        public PriceLoaderJob(IPriceScraperRunner priceLoaderRunner)
        {
            _priceLoaderRunner = priceLoaderRunner;
        }

        public async Task Run(CancellationToken ct)
        {
            await _priceLoaderRunner.LoadAllPrices();
        }
    }
}
