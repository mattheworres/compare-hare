using System;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Api.Features.Prices.Services.Interfaces
{
    public interface IPricePersister : IFeatureService
    {
        void PersistNewPrice(ProductRetailerPrice price, int? currentPriceId);
        void UpdateUnchangedPrice(int currentPriceId, DateTimeOffset today);
    }
}
