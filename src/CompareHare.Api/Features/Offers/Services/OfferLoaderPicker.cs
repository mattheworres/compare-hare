using System;
using CompareHare.Api.Features.Offers.Services.Interfaces;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Entities.Constants;
using CompareHare.Domain.Features.OfferLoaders;
using CompareHare.Domain.Features.OfferLoaders.Interfaces;
using Serilog;

namespace CompareHare.Api.Features.Offers.Services
{
    public class OfferLoaderPicker : IOfferLoaderPicker
    {
        private readonly Lazy<PAPowerOfferLoader> _pAPowerOfferLoader;

        public OfferLoaderPicker(Lazy<PAPowerOfferLoader> pAPowerOfferLoader) {
            _pAPowerOfferLoader = pAPowerOfferLoader;
        }

        public IOfferLoader PickOfferLoader(StateUtilityIndex index)
        {
            switch (index.UtilityState)
            {
                case UtilityStates.Pennsylvania:
                    if (index.UtilityType == UtilityTypes.Power) return this._pAPowerOfferLoader.Value;
                    Log.Logger.Error("OfferLoaderPicker: PA but not power selected");
                    throw new NotImplementedException();

                default:
                    Log.Logger.Error("OfferLoaderPicker: default case reached");
                    throw new NotImplementedException();
            }
        }
    }
}
