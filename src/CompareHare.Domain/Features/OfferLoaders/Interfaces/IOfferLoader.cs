using AngleSharp.Io;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Domain.Features.OfferLoaders.Interfaces
{
    public interface IOfferLoader : IFeatureService
    {
        Task<List<UtilityPrice>> LoadOffers(int utilityIndexId, string loaderIdentifier, IRequester requester = null);
    }
}
