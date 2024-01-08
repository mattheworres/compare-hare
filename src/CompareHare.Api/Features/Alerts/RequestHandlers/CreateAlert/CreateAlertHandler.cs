using AutoMapper;
using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Api.Features.Shared.Services;
using CompareHare.Api.MediatR;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.AlertAssessors.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Alerts.RequestHandlers.CreateAlert
{
    public class CreateAlertHandler : ApiRequestHandlerBase, IRequestHandler<CreateAlertMessage, IActionResult>
    {
        private readonly CompareHareDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IUtilityIndexPersister _suiPersister;
        private readonly Lazy<IAlertAssessor> _alertAssessor;

        public CreateAlertHandler(
            CompareHareDbContext dbContext,
            IMapper mapper,
            IUtilityIndexPersister suiPersister,
            Lazy<IAlertAssessor> alertAssessor) {
            _dbContext = dbContext;
            _mapper = mapper;
            _suiPersister = suiPersister;
            _alertAssessor = alertAssessor;
        }

        public async Task<IActionResult> Handle(CreateAlertMessage message, CancellationToken cancellationToken)
        {
            var model = message.Model;
            //Search for state utility index, if doesnt exist, map and create one
            var suiNeededPersistence = await _suiPersister.DiscoverOrPersistIndex(model);

            //Map to an alert, link to SUI
            var alert = _mapper.Map<Alert>(model);
            alert.StateUtilityIndexId = await _suiPersister.FetchIndexId(model);

            await _dbContext.Alerts.AddAsync(alert);
            await _dbContext.SaveChangesAsync();

            //if we created SUI, return now indicating a need to load offers for SUI
            if (suiNeededPersistence) {
                return Ok(new CreateAlertResponseModel(true, alert.Id));
            }

            //Very purposefully I have split calling just assessor vs needing to call offer loader + assessor
            //for UI reasons, the latter may be really expensive and take a long time.
            var assessorResponse = await _alertAssessor.Value.AssessMatches(alert.Id);

            return Ok(new CreateAlertResponseModel(false, alert.Id, assessorResponseType: (int?)assessorResponse.ReturnType, matchesCount: assessorResponse.MatchesCount));
        }
    }
}
