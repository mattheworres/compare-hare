using CompareHare.Domain.Entities;
using CompareHare.Domain.Features.Authentication.Models;

namespace CompareHare.Domain.Features.Authentication.Interfaces
{
    public interface IUserIdentityModelBuilder
    {
        Task<UserIdentityModel> BuildAsync(User user);
    }
}
