using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;

namespace ServiceLayer.Mapping
{
    public class TrackingMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Tracking, TrackingDto>()
                .ForMember(d => d.RequestIds, opt => opt.Ignore())
                .AfterMap((s, d) =>
                {
                    d.Speed = s.Velocity;
                }); ;

            Mapper.CreateMap<TrackingDto, Tracking>()
                .ForMember(d => d.RequestId, opt => opt.Ignore())
                .AfterMap((s, d) =>
                {
                    d.Velocity = s.Speed;
                });
        }
    }
}
