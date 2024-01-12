using CompareHare.Domain.Features.Authentication.Interfaces;
using MediatR;
using CompareHare.Domain.Features.Authentication.Models;

namespace CompareHare.Api.Features.CurrentUser.RequestHandlers.GetUserIdentity
{
    public class GetUserIdentityHandler : IRequestHandler<GetUserIdentity, UserIdentityModel>
    {
        private readonly IUserIdentityModelBuilder _userIdentityModelBuilder;
        private readonly ICurrentUserProvider _currentUserProvider;

        public GetUserIdentityHandler(
            IUserIdentityModelBuilder userIdentityModelBuilder,
            ICurrentUserProvider currentUserProvider)
        {
            _currentUserProvider = currentUserProvider;
            _userIdentityModelBuilder = userIdentityModelBuilder;
        }

        public async Task<UserIdentityModel> Handle(GetUserIdentity message, CancellationToken cancellationToken)
        {
            var user = await _currentUserProvider.GetUserAsync();

            var model = await _userIdentityModelBuilder.BuildAsync(user);

            return model;
        }
    }
}
