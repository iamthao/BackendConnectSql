using AutoMapper;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.Mapping;

namespace ServiceLayer.Mapping
{
    public class CourierMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Courier, CourierDto>()
                .AfterMap((s, d) =>
                {
                    if (s.User != null)
                    {
                        d.User = s.User.MapTo<UserDto>();
                    }
                });

            Mapper.CreateMap<CourierDto, Courier>()
                .AfterMap((s, d) =>
                {
                    if (s.User != null)
                    {
                        d.User = s.User.MapTo<User>();
                    }
                });
            Mapper.CreateMap<Contact, ContactDto>();
        }
    }
}
