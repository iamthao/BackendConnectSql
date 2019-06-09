using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.Mapping;

namespace ServiceLayer.Mapping
{
    public class RequestHistoryMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<RequestHistory, RequestHistoryDto>()
                .ForMember(o=>o.RequestType, opt=>opt.Ignore())
                .AfterMap((s, d) =>
                {
                    if (s.Request != null)
                    {
                        d.RequestNumber = s.Request.RequestNo;
                        d.Locationfrom = s.Request.LocationFromObj.MapTo<LocationDto>();
                        d.Locationto = s.Request.LocationToObj.MapTo<LocationDto>();
                        d.RequestType = s.Request.Status;
                        d.IsStat = s.Request.IsStat.GetValueOrDefault();
                        d.Description = s.Request.Description;
                        d.IsScheduleCreated = s.Request.HistoryScheduleId.GetValueOrDefault() != 0;
                        d.StartTime = s.Request.StartTime;
                        d.EndTime = s.Request.EndTime;
                        d.ReceivedTime = s.Request.ReceivedTime;
                        d.AcceptedTime = s.Request.AcceptedTime;
                        d.RejectedTime = s.Request.RejectedTime;
                        d.ActualStartTime = s.Request.ActualStartTime;
                        d.ActualEndTime = s.Request.ActualEndTime;
                        d.NoteRequests = s.Request.NoteRequests.OrderByDescending(o=>o.CreateTime).MapTo<NoteDto>();
                        d.RequestId = s.Request.Id;
                        d.IsAgreed = s.Request.IsAgreed;
                        d.CompleteNote = s.Request.CompleteNote;
                        if (s.Request.Signature != null)
                        {
                            d.Signature = Convert.ToBase64String(s.Request.Signature);
                        }
                        if (s.Request.CompletePicture != null)
                        {
                            d.CompletePicture = Convert.ToBase64String(s.Request.CompletePicture);
                        }
                    }
                });

            Mapper.CreateMap<RequestHistoryDto, RequestHistory>()
                .AfterMap((s, d) =>
                {
                    if (s.RequestId!=0)
                    {
                        d.Request=new Request
                        {
                            Id = s.RequestId,
                            RequestNo = s.RequestNumber,
                            LocationFromObj = s.Locationfrom.MapTo<Location>(),
                            LocationToObj = s.Locationto.MapTo<Location>(),
                            Status = s.RequestType,
                            IsStat = s.IsStat,
                            Description = s.Description,
                            StartTime = s.StartTime.GetValueOrDefault(),
                            EndTime = s.EndTime.GetValueOrDefault(),
                            NoteRequests = s.NoteRequests.MapTo<NoteRequest>(),
                            
                        };
                    }
                });
        }
    }
}
