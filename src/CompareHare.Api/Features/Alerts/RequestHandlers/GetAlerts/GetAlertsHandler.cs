using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Api.MediatR;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Authentication.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompareHare.Api.Features.Alerts.RequestHandlers.GetAlerts
{
    public class GetAlertsHandler : ApiRequestHandlerBase, IRequestHandler<GetAlertsMessage, IActionResult>
    {
        private readonly CompareHareDbContext _dbContext;
        private readonly ICurrentUserProvider _currentUserProvider;
        private readonly IMapper _mapper;

        public GetAlertsHandler(
            CompareHareDbContext dbContext,
            ICurrentUserProvider currentUserProvider,
            IMapper mapper) {
            _dbContext = dbContext;
            _currentUserProvider = currentUserProvider;
            _mapper = mapper;
        }

        public async Task<IActionResult> Handle(GetAlertsMessage request, CancellationToken cancellationToken)
        {
            var currentUserId = await _currentUserProvider.GetUserIdAsync();
            var alerts = await _dbContext.Alerts
                .Where(x => x.UserId == currentUserId)
                .Include(x => x.AlertMatch)
                .ThenInclude(y => y.UtilityPriceHistories)
                .Include(x => x.StateUtilityIndex)
                .ToListAsync();

            return Ok(_mapper.Map<IEnumerable<AlertListModel>>(alerts));
        }
    }
}
