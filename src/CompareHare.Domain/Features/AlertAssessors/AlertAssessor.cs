using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Entities.Constants;
using CompareHare.Domain.Features.AlertAssessors.Interfaces;
using CompareHare.Domain.Features.AlertAssessors.Models;
using CompareHare.Domain.Features.AlertAssessors.Queries;
using CompareHare.Domain.Services.Interfaces;
using CompareHare.Domain.Sql.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Domain.Features.AlertAssessors
{
    public class AlertAssessor : IAlertAssessor
    {
        private readonly CompareHareDbContext _dbContext;
        private readonly ISqlExecutor _sqlExecutor;
        private readonly IUtilityPriceHasherHelper _hasherHelper;
        private readonly IMapper _mapper;

        public AlertAssessor(
            CompareHareDbContext dbContext,
            ISqlExecutor sqlExecutor,
            IUtilityPriceHasherHelper hasherHelper,
            IMapper mapper) {
            _dbContext = dbContext;
            _sqlExecutor = sqlExecutor;
            _hasherHelper = hasherHelper;
            _mapper = mapper;
        }

        public async Task<AlertAssessorReturnModel> AssessMatches(int alertId)
        {
            var returnModel = new AlertAssessorReturnModel();
            var alert = await _dbContext.Alerts.FindAsync(alertId);

            if (alert == null) throw new InvalidOperationException("Alert not found");

            var sui = await _dbContext.StateUtilityIndices.FindAsync(alert.StateUtilityIndexId);

            if (!string.IsNullOrEmpty(alert.StateUtilityIndexHash) && alert.StateUtilityIndexHash == sui.LastUpdatedHash) {
                returnModel.ReturnType = AlertAssessorReturnType.NoChangeAtStateIndexLevel;
                return returnModel;
            }

            var matchingHistoryIds = await GetMatches(alert, sui);

            var match = await _dbContext.AlertMatches.FirstOrDefaultAsync(x => x.AlertId == alert.Id);

            if (match == null && matchingHistoryIds.Count() == 0) {
                returnModel.ReturnType = AlertAssessorReturnType.NoNewMatchesFound;
                return returnModel;
            } else if (match == null) {
                match = new AlertMatch() {
                    AlertId = alert.Id,
                    UserId = alert.UserId,
                    StateUtilityIndexId = sui.Id,
                };

                await _dbContext.AlertMatches.AddAsync(match);
            }

            var matchingHistories = new List<UtilityPriceHistory>();

            foreach(var id in matchingHistoryIds) {
                matchingHistories.Add(await _dbContext.UtilityPriceHistories.FindAsync(id));
            }

            if (_hasherHelper.AreOffersDifferent(matchingHistories, match.AlertOfferHash)) {
                RemoveExistingHistories(match);

                match.UtilityPriceHistories = _mapper.Map<IEnumerable<AlertMatchUtilityPriceHistory>>(matchingHistories);
                match.AlertOfferHash = _hasherHelper.GetModelHash(matchingHistories);

                await _dbContext.SaveChangesAsync();

                returnModel.ReturnType = AlertAssessorReturnType.NewMatchesAvailable;
                returnModel.UpdatedMatches = matchingHistories;

                return returnModel;
            } else {
                returnModel.ReturnType = AlertAssessorReturnType.NoChangeAtMatchLevel;

                return returnModel;
            }
        }

        private async Task<IEnumerable<int>> GetMatches(Alert alert, StateUtilityIndex sui) {
            if (sui.UtilityState == UtilityStates.Pennsylvania && sui.UtilityType == UtilityTypes.Power) {
                return await _sqlExecutor.Execute(new GetAlertMatchesForPaPower(alert, sui.Id));
            }

            throw new InvalidOperationException("State or utility not supported");
        }

        private void RemoveExistingHistories(AlertMatch match) {
            if (match.UtilityPriceHistories.Any() == false) return;

            _dbContext.RemoveRange(match.UtilityPriceHistories);
        }
    }
}
