using System.Collections.Generic;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Api.Features.Offers.Services.Interfaces
{
    public interface IUtilityPriceHasherHelper : IFeatureService
    {
         bool AreOffersDifferent(IEnumerable<UtilityPrice> utilityPrices, string existingHash);
         string GetModelHash(IEnumerable<UtilityPrice> utilityPrices);
    }
}
