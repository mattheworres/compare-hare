using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CompareHare.Api.Features.Authentication.RequestHandlers.LogOut
{
    public class LogOutMessage : IRequest<IActionResult> { }
}
