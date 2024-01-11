using System.Security.Claims;
using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Authentication.Interfaces;
using CompareHare.Domain.Features.Interfaces;

namespace CompareHare.Api.Features.Shared.Services
{
    public class CurrentUserProvider : ICurrentUserProvider, IFeatureService
    {
        //TODO: Make sure this is actually how we should be getting the current user
        // private readonly ClaimsPrincipal _principal;
        private readonly IHttpContextAccessor _httpContextAccessor;
        // private readonly UserManager<User> _userManager;
        private readonly Lazy<CompareHareDbContext> _dbContext;
        private const string EXCEPTION_TEXT = "User not authenticated";

        public CurrentUserProvider(
            // ClaimsPrincipal principal,
            IHttpContextAccessor httpContextAccessor,
            // UserManager<User> userManager,
            Lazy<CompareHareDbContext> dbContext)
        {
            // _principal = principal;
            _httpContextAccessor = httpContextAccessor;
            // _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<User> GetUserAsync()
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString)) {
                throw new Exception(EXCEPTION_TEXT);
            }

            var userId = Convert.ToInt32(userIdString);

            if (userId == 0) {
                throw new Exception(EXCEPTION_TEXT);
            }

            var user = await _dbContext.Value.Users.FindAsync(userId);

            return user ?? new User();
        }

        public async Task<int> GetUserIdAsync()
        {
            throw new NotImplementedException();
            // TODO: remove once most of biz logic is here
            // var user = await _userManager.GetUserAsync(_principal);
            // return user.Id;
        }

        // For contexts in which async isn't available; Namely: automapper resolvers
        public User GetUserSync()
        {
            var userIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString)) {
                throw new Exception(EXCEPTION_TEXT);
            }

            var userId = Convert.ToInt32(userIdString);

            if (userId == 0) {
                throw new Exception(EXCEPTION_TEXT);
            }

            var user = _dbContext.Value.Users.Find(userId);

            if (user == null) {
                throw new Exception(EXCEPTION_TEXT);
            }

            return user;
        }

        public int GetUserIdSync()
        {
            var userId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Convert.ToInt32(userId);
        }
    }
}
