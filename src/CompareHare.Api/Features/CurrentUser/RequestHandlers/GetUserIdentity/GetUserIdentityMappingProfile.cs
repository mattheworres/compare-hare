using CompareHare.Domain.Entities;
using AutoMapper;
using CompareHare.Domain.Features.Authentication.Models;

namespace CompareHare.Api.Features.CurrentUser.RequestHandlers.GetUserIdentity
{
    public class GetUserIdentityMappingProfile : Profile
    {
        public GetUserIdentityMappingProfile()
        {
            CreateMap<User, UserIdentityModel>()
                .ForMember(d => d.Roles, mce => mce.Ignore())
            ;
        }
    }
}
