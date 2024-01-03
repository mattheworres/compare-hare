using CompareHare.Domain.Entities;

namespace CompareHare.Domain.Features.Authentication.Interfaces
{
    public interface ICurrentUserProvider
    {
        Task<User> GetUserAsync();
        Task<int> GetUserIdAsync();
        User GetUserSync();
        int GetUserIdSync();
    }
}
