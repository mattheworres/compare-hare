using CompareHare.Api.Controllers;
using CompareHare.Api.Features.Authentication.Models;
using CompareHare.Api.Features.Authentication.RequestHandlers.LogIn;
using CompareHare.Api.Features.Authentication.RequestHandlers.LogOut;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Authentication
{
    public class AuthenticationController : SpaApiController
    {
        private readonly IMediator _mediator;

        public AuthenticationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("sign-in"), HttpPost, AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] LogInModel model)
        {
            var remoteIpAddress = Request.HttpContext.Connection.RemoteIpAddress;
            var ip = remoteIpAddress != null ? remoteIpAddress.ToString() : "";
            var valid = ModelState.IsValid;
            var errors = ModelState.Values;
            return await _mediator.Send(new LogInMessage(model, ip));
        }

        [HttpDelete("log-out"), AllowAnonymous]
        public async Task LogOut()
        {
            await _mediator.Send(new LogOutMessage());
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
