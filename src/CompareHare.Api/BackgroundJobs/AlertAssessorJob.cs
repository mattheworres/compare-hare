using CompareHare.Api.BackgroundJobs.Interfaces;
using CompareHare.Api.Features.Alerts.Services.Interfaces;

namespace CompareHare.Api.BackgroundJobs
{
    public class AlertAssessorJob : IAlertAssessorJob
    {
        private readonly IAlertAssessorRunner _alertAssessorRunner;

        public AlertAssessorJob(IAlertAssessorRunner alertAssessorRunner)
        {
            _alertAssessorRunner = alertAssessorRunner;
        }

        public async Task Run(CancellationToken ct)
        {
            await _alertAssessorRunner.AssessAllOffers();
        }
    }
}
