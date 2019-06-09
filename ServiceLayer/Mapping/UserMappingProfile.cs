using System;
using AutoMapper;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;

namespace ServiceLayer.Mapping
{
    public class UserMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<User, UserDto>()
                .ForMember(d => d.Avatar, opt => opt.Ignore())
                .AfterMap((s, d) =>
                {
                    if (s.Avatar != null)
                    {
                        d.AvatarInBase64 = Convert.ToBase64String(s.Avatar);
                        //d.Avatar = "data:image/jpg;base64," + Convert.ToBase64String(s.Avatar);
                    }
                });

            Mapper.CreateMap<UserDto, User>()
                .ForMember(d => d.Password, opt => opt.Ignore())
                .ForMember(d => d.Avatar, opt => opt.Ignore())
                .AfterMap((s, d) =>
                {
                    if (s.AvatarInBase64 != null)
                    {
                        d.Avatar = Convert.FromBase64String(s.AvatarInBase64);
                    }
                });
        }
    }
}
