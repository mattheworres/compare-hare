using System;
using System.Security.Claims;
using System.Threading.Tasks;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Authentication.Interfaces;
using CompareHare.Domain.Features.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CompareHare.Api.Features.Shared.Services
{
    public class CurrentUserProvider : ICurrentUserProvider, IFeatureService
    {
        private readonly ClaimsPrincipal _principal;
        private readonly UserManager<User> _userManager;
        private readonly Lazy<CompareHareDbContext> _dbContext;

        public CurrentUserProvider(
            ClaimsPrincipal principal,
            UserManager<User> userManager,
            Lazy<CompareHareDbContext> dbContext)
        {
            _principal = principal;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<User> GetUserAsync()
        {
            if (!_principal.Identity.IsAuthenticated) return null;

            return await _userManager.GetUserAsync(_principal);
        }

        public async Task<int> GetUserIdAsync()
        {
            var user = await _userManager.GetUserAsync(_principal);
            return user.Id;
        }

        // For contexts in which async isn't available; Namely: automapper resolvers
        public User GetUserSync()
        {
            if (!_principal.Identity.IsAuthenticated) return null;

            var userId = _userManager.GetUserId(_principal);

            return _dbContext.Value.Users.Find(Convert.ToInt32(userId));
        }

        public int GetUserIdSync()
        {
            if (!_principal.Identity.IsAuthenticated) return 0;

            return Convert.ToInt32(_userManager.GetUserId(_principal));
        }
    }
}
