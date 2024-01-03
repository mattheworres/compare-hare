using System.Threading;
using System.Threading.Tasks;
using CompareHare.Api.MediatR;
using CompareHare.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Authentication.RequestHandlers.LogOut
{
    public class LogOutHandler : ApiRequestHandlerBase, IRequestHandler<LogOutMessage, IActionResult>
    {
        private readonly SignInManager<User> _signInManager;

        public LogOutHandler(
            SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<IActionResult> Handle(LogOutMessage message, CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }
    }
}
