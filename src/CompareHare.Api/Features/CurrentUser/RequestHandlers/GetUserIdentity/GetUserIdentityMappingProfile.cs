using CompareHare.Api.Domain.Features.Authentication.Models;
using CompareHare.Domain.Entities;
using AutoMapper;

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
