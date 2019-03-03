using System;
using System.Linq;
using System.Threading.Tasks;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Entities.Constants;
using CompareHare.Domain.Features.Interfaces;
using CompareHare.Domain.Features.OfferLoaders.Interfaces;
using CompareHare.Api.Features.Offers.Services.Interfaces;
using CompareHare.Domain.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;
using CompareHare.Domain.Features.OfferLoaders;
using RateLimiter;

namespace CompareHare.Api.Features.OfferLoaders
{
    public class OfferLoaderRunner : IOfferLoaderRunner, IFeatureService
    {
        //TODO: Make this an appsetting option:
        private const int THROTTLE_SECONDS = 15;

        private readonly CompareHareDbContext _dbContext;
        private readonly Lazy<PAPowerOfferLoader> _pAPowerOfferLoader;
        private readonly IObjectHasher _objectHasher;
        private readonly IOfferPersister _offerPersister;
        private readonly IUtilityPriceHasherHelper _hasherHelper;

        public OfferLoaderRunner(
            CompareHareDbContext dbContext,
            Lazy<PAPowerOfferLoader> pAPowerOfferLoader,
            IObjectHasher objectHasher,
            IOfferPersister offerPersister,
            IUtilityPriceHasherHelper hasherHelper)
        {
            _dbContext = dbContext;
            _pAPowerOfferLoader = pAPowerOfferLoader;
            _objectHasher = objectHasher;
            _offerPersister = offerPersister;
            _hasherHelper = hasherHelper;
        }

        public async Task LoadAllOffers()
        {
            var indices = await _dbContext.StateUtilityIndices.OrderBy(x => x.UtilityState).ThenBy(x => x.UtilityType).ToListAsync();

            foreach (var index in indices)
            {
                _dbContext.Entry(index).State = EntityState.Detached;
                await _dbContext.SaveChangesAsync();
            }

            //Log.Logger.Information("Ok, got indices... {0}", indices.Count());

            var limiter = TimeLimiter.GetFromMaxCountByInterval(1, TimeSpan.FromSeconds(THROTTLE_SECONDS));

            foreach(var index in indices) {
                await limiter.Perform(() => LoadOffersForIndex(index));
            }
        }

        private IOfferLoader PickOfferLoader(StateUtilityIndex index) {
            switch(index.UtilityState) {
                case UtilityStates.Pennsylvania:
                    if (index.UtilityType == UtilityTypes.Power) return this._pAPowerOfferLoader.Value;
                    return null;

                default:
                return null;
            }
        }

        private async Task LoadOffersForIndex(StateUtilityIndex index) {
            try
            {
                var offerLoader = PickOfferLoader(index);

                //Log.Logger.Information("Ok, got a loader for {0}... {1}", index.LoaderDataIdentifier, offerLoader.GetType());

                if (offerLoader == null) return;

                var offers = await offerLoader.LoadOffers(index.Id, index.LoaderDataIdentifier);

                //Log.Logger.Information("Ok, got some offers... {0}", offers.Count());

                if (offers.Count() == 0) return;

                var offerHash = _hasherHelper.GetModelHash(offers);

                //Log.Logger.Information("Ok, got a hash, comparing it to ... ({0}) and ({1})", offerHash, index.LastUpdatedHash);

                if (string.IsNullOrEmpty(index.LastUpdatedHash) || offerHash != index.LastUpdatedHash)
                {
                    await _offerPersister.PersistNewOffers(offers, index.Id, offerHash);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Error in offer loader runner for state {0} utility {1} identifier {2}", index.UtilityState.ToString(), index.UtilityType.ToString(), index.LoaderDataIdentifier);
            }
        }
    }
}
