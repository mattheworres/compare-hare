using System.Threading;
using System.Threading.Tasks;
using CompareHare.Api.BackgroundJobs.Interfaces;
using CompareHare.Api.Features.Alerts.Services.Interfaces;

namespace CompareHare.Api.BackgroundJobs
{
    public class AlertAssessorJob : IAlertAssessorJob
    {
        private readonly IAlertAssessorRunner _offerLoaderRunner;

        public AlertAssessorJob(IAlertAssessorRunner offerLoaderRunner) {
            _offerLoaderRunner = offerLoaderRunner;
        }

        public async Task Run(CancellationToken ct)
        {
            await _offerLoaderRunner.AssessAllOffers();
        }
    }
}
