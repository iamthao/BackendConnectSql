using System;
using AutoMapper;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;

namespace ServiceLayer.Mapping
{
    public class NoteRequestMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<NoteRequest, NoteDto>()
                .AfterMap((s, d) =>
                {
                    d.Date = s.CreateTime;
                });

            Mapper.CreateMap<NoteDto, NoteRequest>()
                .AfterMap((s, d) =>
                {
                    d.CreateTime = s.Date;
                });
        }
    }
}
