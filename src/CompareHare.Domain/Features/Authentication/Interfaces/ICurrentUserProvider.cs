using CompareHare.Domain.Entities;

namespace CompareHare.Domain.Features.Authentication.Interfaces
{
    public interface ICurrentUserProvider
    {
        Task<User> GetUserAsync();
        int GetUserIdSync();
        User GetUserSync();
    }
}
