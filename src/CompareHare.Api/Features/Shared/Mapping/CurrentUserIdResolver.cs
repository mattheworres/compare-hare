using AutoMapper;
using CompareHare.Domain.Features.Authentication.Interfaces;

namespace CompareHare.Api.Features.Shared.Mapping
{
    public class CurrentUserIdResolver : IValueResolver<object, object, int>
    {
        private readonly ICurrentUserProvider _currentUserProvider;

        public CurrentUserIdResolver(ICurrentUserProvider currentUserProvider) {
            _currentUserProvider = currentUserProvider;
        }

        public int Resolve(object source, object destination, int destMember, ResolutionContext context)
        {
            return _currentUserProvider.GetUserIdSync();
        }
    }
}
