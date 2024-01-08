using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Api.Features.Alerts.Services.Interfaces
{
    public interface IAlertAssessorRunner : IFeatureService
    {
        Task AssessAllOffers();
    }
}
