using CompareHare.Domain.Features.Authentication.Models;
using MediatR;

namespace CompareHare.Api.Features.CurrentUser.RequestHandlers.GetUserIdentity
{
    public class GetUserIdentity : IRequest<UserIdentityModel> {}
}
