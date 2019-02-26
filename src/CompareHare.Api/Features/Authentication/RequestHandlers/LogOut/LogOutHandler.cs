using System.Threading;
using System.Threading.Tasks;
using CompareHare.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CompareHare.Api.Features.Authentication.RequestHandlers.LogOut
{
    public class LogOutHandler : IRequestHandler<LogOut>
    {
        private readonly SignInManager<User> _signInManager;

        public LogOutHandler(
            SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task Handle(LogOut message, CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();
        }
    }
}
