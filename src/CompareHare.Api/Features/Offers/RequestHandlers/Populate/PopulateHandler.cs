using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CompareHare.Api.Features.Offers.Services.Interfaces;
using CompareHare.Api.MediatR;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.AlertAssessors.Interfaces;
using CompareHare.Domain.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Api.Features.Offers.RequestHandlers.Populate
{
    public class PopulateHandler : ApiRequestHandlerBase, IRequestHandler<PopulateMessage, IActionResult>
    {
        private readonly CompareHareDbContext _dbContext;
        private readonly IAlertAssessor _alertAssessor;
        private readonly IOfferLoaderPicker _offerLoaderPicker;
        private readonly IUtilityPriceHasherHelper _hasherHelper;
        private readonly IOfferPersister _offerPersister;

        public PopulateHandler(
            CompareHareDbContext dbContext,
            IAlertAssessor alertAssessor,
            IOfferLoaderPicker offerLoaderPicker,
            IUtilityPriceHasherHelper hasherHelper,
            IOfferPersister offerPersister) {
            _dbContext = dbContext;
            _alertAssessor = alertAssessor;
            _offerLoaderPicker = offerLoaderPicker;
            _hasherHelper = hasherHelper;
            _offerPersister = offerPersister;
        }

        public async Task<IActionResult> Handle(PopulateMessage message, CancellationToken cancellationToken)
        {
            var alertId = message.Model.AlertId;
            var alert = await _dbContext.Alerts.FindAsync(alertId);
            var sui = await _dbContext.StateUtilityIndices.FindAsync(alert.StateUtilityIndexId);

            //We only wish to "populate" offers for this SUI if they haven't been populated (ie the SUI was just created/reactivated):
            if (string.IsNullOrEmpty(sui.LastUpdatedHash)) {
                var offerLoader = _offerLoaderPicker.PickOfferLoader(sui);

                var newOffers = await offerLoader.LoadOffers(sui.Id, sui.LoaderDataIdentifier);

                if (newOffers.Any()) {
                    var offerHash = _hasherHelper.GetModelHash(newOffers);
                    await _offerPersister.PersistNewOffers(newOffers, sui.Id, offerHash);
                }
            }

            var assessorResponse = await _alertAssessor.AssessMatches(alertId);

            //Now that the assessor ran, lets make sure its got the most up to date hash from the SUI
            alert.StateUtilityIndexHash = await _dbContext.StateUtilityIndices.Where(x => x.Id == sui.Id).Select(x => x.LastUpdatedHash).FirstOrDefaultAsync();
            await _dbContext.SaveChangesAsync();

            return Ok(assessorResponse);
        }
    }
}
