using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Api.MediatR;
using CompareHare.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Api.Features.Alerts.RequestHandlers.GetAlert
{
    public class GetAlertHandler : ApiRequestHandlerBase, IRequestHandler<GetAlertMessage, IActionResult>
    {
        private readonly CompareHareDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetAlertHandler(CompareHareDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetAlertMessage message, CancellationToken cancellationToken)
        {
            var alert = await _dbContext.Alerts
                .Where(x => x.Id == message.Model.AlertId)
                .Include(x => x.StateUtilityIndex)
                .Include(x => x.AlertMatch)
                .ThenInclude(x => x.UtilityPriceHistories)
                .ThenInclude(x => x.UtilityPriceHistory)
                .FirstAsync();

            var model = _mapper.Map<AlertDisplayModel>(alert);

            model.Prices = new List<PriceDisplayModel>();

            if (alert.AlertMatch != null && alert.AlertMatch.UtilityPriceHistories.Any()) {
                foreach(var uph in alert.AlertMatch.UtilityPriceHistories) {
                    if (uph.UtilityPriceHistory != null) {
                        model.Prices.Add(_mapper.Map<PriceDisplayModel>(uph.UtilityPriceHistory));
                    }
                }
            }

            return Ok(model);
        }
    }
}
