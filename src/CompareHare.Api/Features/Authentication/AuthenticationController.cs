#region usings

using System.Threading.Tasks;
using CompareHare.Api.Controllers;
using CompareHare.Api.Features.Authentication.Models;
using CompareHare.Api.Features.Authentication.RequestHandlers.LogIn;
using CompareHare.Api.Features.Authentication.RequestHandlers.LogOut;
// using CompareHare.Api.Features.Authentication.RequestHandlers.ForgotPassword;
// using CompareHare.Api.Features.Authentication.RequestHandlers.ResetPassword;
// using CompareHare.Api.Features.Authentication.RequestHandlers.ChangePassword;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace CompareHare.Api.Features.Authentication
{
    [Route("api/[controller]")]
    public class AuthenticationController : SpaApiController
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("log-in"), HttpPost, AllowAnonymous]
        public async Task<IActionResult> LogIn([FromBody] LogInModel model)
        {
            var ip = this.Request.HttpContext.Connection.RemoteIpAddress;
            return await _mediator.Send(new LogInMessage(model, ip.ToString()));
        }

        [HttpDelete("log-out"), AllowAnonymous]
        public async Task LogOut()
        {
            await _mediator.Send(new LogOut());
        }

        // [Route("forgot-password"), HttpPost, AllowAnonymous]
        // public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        // {
        //     return await _mediator.Send(new ForgotPasswordMessage(model));
        // }

        // [Route("reset-password"), HttpPost, AllowAnonymous]
        // public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        // {
        //     return await _mediator.Send(new ResetPasswordMessage(model));
        // }

        // [Route("change-password"), HttpPut]
        // public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model) {
        //     return await _mediator.Send(new ChangePasswordMessage(model));
        // }
    }
}
