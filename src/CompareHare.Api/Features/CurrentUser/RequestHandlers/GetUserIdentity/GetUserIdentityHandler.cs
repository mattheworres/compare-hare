using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using CompareHare.Api.Domain.Features.Authentication.Models;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Authentication.Interfaces;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CompareHare.Api.Features.CurrentUser.RequestHandlers.GetUserIdentity
{
    public class GetUserIdentityHandler : IRequestHandler<GetUserIdentity, UserIdentityModel>
    {
        private readonly ClaimsPrincipal _claimsPrincipal;
        private readonly IMapper _mapper;
        private readonly IUserIdentityModelBuilder _userIdentityModelBuilder;
        private readonly UserManager<User> _userManager;

        public GetUserIdentityHandler(
            UserManager<User> userManager,
            ClaimsPrincipal claimsPrincipal,
            IMapper mapper,
            IUserIdentityModelBuilder userIdentityModelBuilder)
        {
            _userManager = userManager;
            _claimsPrincipal = claimsPrincipal;
            _mapper = mapper;
            _userIdentityModelBuilder = userIdentityModelBuilder;
        }

        public async Task<UserIdentityModel> Handle(GetUserIdentity message, CancellationToken cancellationToken)
        {
            var user = await _userManager.GetUserAsync(_claimsPrincipal);

            var model = await _userIdentityModelBuilder.BuildAsync(user);

            return model;
        }
    }
}
