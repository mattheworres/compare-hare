using System.Threading.Tasks;
using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Api.Features.Shared.Services
{
    public interface IUtilityIndexPersister : IFeatureService
    {
         Task<bool> DiscoverOrPersistIndex(CreateAlertModel model);
         Task<int> FetchIndexId(CreateAlertModel model);
    }
}
