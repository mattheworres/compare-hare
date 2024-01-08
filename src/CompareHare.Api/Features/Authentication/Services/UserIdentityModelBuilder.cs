
using CompareHare.Domain.Features.Authentication.Models;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Authentication.Interfaces;
using CompareHare.Domain.Features.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace CompareHare.Api.Features.Authentication.Services
{
    public class UserIdentityModelBuilder : IUserIdentityModelBuilder, IFeatureService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserIdentityModelBuilder(
            UserManager<User> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<UserIdentityModel> BuildAsync(User user)
        {
            var responseModel = _mapper.Map<UserIdentityModel>(user);
            responseModel.Roles = await _userManager.GetRolesAsync(user);
            return responseModel;
        }
    }
}
