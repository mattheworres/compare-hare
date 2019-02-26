using System.Threading.Tasks;
using CompareHare.Api.Domain.Features.Authentication.Models;
using CompareHare.Domain.Entities;

namespace CompareHare.Domain.Features.Authentication.Interfaces
{
    public interface IUserIdentityModelBuilder
    {
        Task<UserIdentityModel> BuildAsync(User user);
    }
}
