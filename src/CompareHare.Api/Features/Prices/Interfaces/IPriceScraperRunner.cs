using System.Threading.Tasks;
using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Api.Features.Prices.Interfaces
{
    public interface IPriceScraperRunner : IFeatureService
    {
        Task LoadAllPrices();
    }
}