using CompareHare.Api.Controllers;
using CompareHare.Api.Features.CurrentUser.RequestHandlers.GetUserIdentity;
using CompareHare.Domain.Features.Authentication.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.CurrentUser
{
    [Route("api/current-user")]
    public class CurrentUserController : SpaApiController
    {
        private readonly IMediator _mediator;

        public CurrentUserController(
            IMediator mediator) => _mediator = mediator;

        [HttpGet("identity")]
        public async Task<UserIdentityModel> Identity() {
            return await _mediator.Send(new GetUserIdentity());
        }
    }
}
