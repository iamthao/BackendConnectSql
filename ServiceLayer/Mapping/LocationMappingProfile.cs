using AutoMapper;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;

namespace ServiceLayer.Mapping
{
    public class LocationMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Location, LocationDto>()
                .AfterMap((s, d) =>
                {
                    d.State = s.StateOrProvinceOrRegion;
                });

            Mapper.CreateMap<LocationDto, Location>();
        }
    }
}
