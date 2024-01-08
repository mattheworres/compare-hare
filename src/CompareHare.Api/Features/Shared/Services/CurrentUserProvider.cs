using System.Security.Claims;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Authentication.Interfaces;
using CompareHare.Domain.Features.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CompareHare.Api.Features.Shared.Services
{
    public class CurrentUserProvider : ICurrentUserProvider, IFeatureService
    {
        // private readonly ClaimsPrincipal _principal;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly Lazy<CompareHareDbContext> _dbContext;

        public CurrentUserProvider(
            // ClaimsPrincipal principal,
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager,
            Lazy<CompareHareDbContext> dbContext)
        {
            // _principal = principal;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<User> GetUserAsync()
        {
            throw new NotImplementedException();
            // if (_principal.Identity == null || !_principal.Identity.IsAuthenticated) return null;

            // return await _userManager.GetUserAsync(_principal);
        }

        public async Task<int> GetUserIdAsync()
        {
            throw new NotImplementedException();
            // var user = await _userManager.GetUserAsync(_principal);
            // return user.Id;
        }

        // For contexts in which async isn't available; Namely: automapper resolvers
        public User GetUserSync()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return _dbContext.Value.Users.Find(Convert.ToInt32(userId));
            // if (_principal.Identity == null || !_principal.Identity.IsAuthenticated) return null;

            // var userId = _userManager.GetUserId(_principal);

            // return _dbContext.Value.Users.Find(Convert.ToInt32(userId));
        }

        public int GetUserIdSync()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Convert.ToInt32(userId);
            // if (_principal.Identity == null || !_principal.Identity.IsAuthenticated) return 0;

            // return Convert.ToInt32(_userManager.GetUserId(_principal));
        }
    }
}
