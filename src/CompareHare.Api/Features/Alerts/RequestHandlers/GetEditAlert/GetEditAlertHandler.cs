using System.Threading;

using AutoMapper;
using CompareHare.Api.Features.Alerts.Models;
using CompareHare.Api.MediatR;
using CompareHare.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Alerts.RequestHandlers.GetEditAlert
{
  public class GetEditAlertHandler : ApiRequestHandlerBase, IRequestHandler<GetEditAlertMessage, IActionResult>
  {
    private readonly CompareHareDbContext _dbContext;
    private readonly IMapper _mapper;

    public GetEditAlertHandler(CompareHareDbContext dbContext, IMapper mapper)
    {
      _dbContext = dbContext;
      _mapper = mapper;
    }

    public async Task<IActionResult> Handle(GetEditAlertMessage message, CancellationToken cancellationToken)
    {
      var alert = await _dbContext.Alerts.FindAsync(message.Model.Id);

      return Ok(_mapper.Map<EditAlertModel>(alert));
    }
  }
}
