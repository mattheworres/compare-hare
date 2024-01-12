using CompareHare.Api.BackgroundJobs.Interfaces;
using CompareHare.Api.Features.Offers.Services.Interfaces;

namespace CompareHare.Api.BackgroundJobs
{
    public class OfferLoaderJob : IOfferLoaderJob
    {
        private readonly IOfferLoaderRunner _offerLoaderRunner;

        public OfferLoaderJob(IOfferLoaderRunner offerLoaderRunner) {
            _offerLoaderRunner = offerLoaderRunner;
        }

        public async Task Run(CancellationToken ct)
        {
            await _offerLoaderRunner.LoadAllOffers();
        }
    }
}
