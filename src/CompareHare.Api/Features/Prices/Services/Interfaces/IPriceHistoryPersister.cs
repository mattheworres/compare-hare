using System.Threading.Tasks;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Api.Features.Prices.Services.Interfaces
{
    public interface IPriceHistoryPersister : IFeatureService
    {
        Task PersistNewPriceHistory(ProductRetailerPriceHistory history);
    }
}