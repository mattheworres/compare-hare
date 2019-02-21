#region usings

using System.Threading;
using System.Threading.Tasks;
using CompareHare.Api.MediatR;
using CompareHare.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System;
using CompareHare.Domain.Features.Authentication.Interfaces;

#endregion

namespace CompareHare.Api.Features.Authentication.RequestHandlers.LogIn
{
    public class LogInHandler : ApiRequestHandlerBase, IRequestHandler<LogInMessage, IActionResult>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly CompareHareDbContext _dbContext;
        private readonly IUserIdentityModelBuilder _userIdentityModelBuilder;

        public LogInHandler(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            CompareHareDbContext dbContext,
            IUserIdentityModelBuilder userIdentityModelBuilder)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContext = dbContext;
            _userIdentityModelBuilder = userIdentityModelBuilder;
        }

        public async Task<IActionResult> Handle(LogInMessage message, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(message.Model.Email);

            if(!user.FirstLogin.HasValue)
                user.FirstLogin = DateTime.UtcNow;

            user.LastLogin = DateTime.UtcNow;

            await _signInManager.SignInAsync(user, false);

            await _userManager.ResetAccessFailedCountAsync(user);

            _dbContext.SaveChanges();

            var model = await _userIdentityModelBuilder.BuildAsync(user);
            model.Roles = await _userManager.GetRolesAsync(user);

            return Ok(model);
        }
    }
}
