using System.Threading;
using System.Threading.Tasks;
using CompareHare.Api.Features.Offers.Services.Interfaces;
using CompareHare.Api.MediatR;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.AlertAssessors.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Offers.RequestHandlers.Populate
{
    public class PopulateHandler : ApiRequestHandlerBase, IRequestHandler<PopulateMessage, IActionResult>
    {
        private readonly CompareHareDbContext _dbContext;
        private readonly IAlertAssessor _alertAssessor;
        private readonly IOfferLoaderPicker _offerLoaderPicker;

        public PopulateHandler(CompareHareDbContext dbContext, IAlertAssessor alertAssessor, IOfferLoaderPicker offerLoaderPicker) {
            _dbContext = dbContext;
            _alertAssessor = alertAssessor;
            _offerLoaderPicker = offerLoaderPicker;
        }

        public async Task<IActionResult> Handle(PopulateMessage message, CancellationToken cancellationToken)
        {
            var alertId = message.Model.AlertId;
            var alert = await _dbContext.Alerts.FindAsync(alertId);
            var sui = await _dbContext.StateUtilityIndices.FindAsync(alert.StateUtilityIndexId);

            var offerLoader = _offerLoaderPicker.PickOfferLoader(sui);

            await offerLoader.LoadOffers(sui.Id, sui.LoaderDataIdentifier);

            var assessorResponse = await _alertAssessor.AssessMatches(alertId);

            return Ok(assessorResponse);
        }
    }
}
