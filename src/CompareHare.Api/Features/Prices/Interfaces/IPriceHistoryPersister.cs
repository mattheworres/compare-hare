using System.Threading.Tasks;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Api.Features.Prices.Interfaces
{
    public interface IPriceHistoryPersister : IFeatureService
    {
        Task PersistNewPriceHistory(ProductRetailerPriceHistory history);
    }
}