using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Interfaces;
using CompareHare.Domain.Features.OfferLoaders.Interfaces;

namespace CompareHare.Api.Features.Offers.Services.Interfaces
{
    public interface IOfferLoaderPicker : IFeatureService
    {
         IOfferLoader PickOfferLoader(StateUtilityIndex index);
    }
}
