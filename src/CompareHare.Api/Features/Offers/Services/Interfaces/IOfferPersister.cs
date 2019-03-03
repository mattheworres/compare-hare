using System.Collections.Generic;
using System.Threading.Tasks;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Api.Features.Offers.Services.Interfaces
{
    public interface IOfferPersister : IFeatureService
    {
         Task PersistNewOffers(IEnumerable<UtilityPrice> utilityPrices, int utilityIndexId, string offerHash);
    }
}
