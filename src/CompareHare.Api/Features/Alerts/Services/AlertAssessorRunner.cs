

using CompareHare.Api.Features.Alerts.Services.Interfaces;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.AlertAssessors.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CompareHare.Api.Features.Alerts.Services
{
    public class AlertAssessorRunner : IAlertAssessorRunner
    {
        private readonly CompareHareDbContext _dbContext;
        private readonly IAlertAssessor _assessor;

        public AlertAssessorRunner(CompareHareDbContext dbContext, IAlertAssessor assessor)
        {
            _assessor = assessor;
            _dbContext = dbContext;
        }

        public async Task AssessAllOffers()
        {
            var alerts = await _dbContext.Alerts.Where(x => x.Active).ToListAsync();

            Log.Logger.Information($"AlertAssessorRunner: Ok, we have {alerts.Count()} alerts here");

            foreach (var alert in alerts)
            {
                await _assessor.AssessMatches(alert.Id);
            }
        }
    }
}
