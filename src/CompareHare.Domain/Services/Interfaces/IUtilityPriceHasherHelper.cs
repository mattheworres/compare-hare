using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Domain.Services.Interfaces
{
    public interface IUtilityPriceHasherHelper : IFeatureService
    {
        bool AreOffersDifferent(IEnumerable<UtilityPrice> utilityPrices, string existingHash);
        string GetModelHash(IEnumerable<UtilityPrice> utilityPrices);
        bool AreOffersDifferent(IEnumerable<UtilityPriceHistory> utilityPrices, string existingHash);
        string GetModelHash(IEnumerable<UtilityPriceHistory> utilityPrices);
    }
}
