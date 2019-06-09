using AutoMapper;
using Framework.DomainModel.ValueObject;
using Framework.Mapping;
using QuickspatchWeb.Models.Schedule;

namespace QuickspatchWeb.Models.Mapping
{
    public class ScheduleMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Framework.DomainModel.Entities.Schedule, DashboardScheduleDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardScheduleShareViewModel>();
            });
            Mapper.CreateMap<Framework.DomainModel.Entities.Schedule, DashboardScheduleShareViewModel>()
                .AfterMap((s, d) =>
                {
                    if (s.LocationFromObj != null)
                    {
                        d.LocationFromDataSource = new LookupItemVo
                        {
                            KeyId = s.LocationFromObj.Id,
                            DisplayName = s.LocationFromObj.Name
                        };
                    }

                    if (s.LocationToObj != null)
                    {
                        d.LocationToDataSource = new LookupItemVo
                        {
                            KeyId = s.LocationToObj.Id,
                            DisplayName = s.LocationToObj.Name
                        };
                    }
                }); 

            Mapper.CreateMap<DashboardScheduleDataViewModel, Framework.DomainModel.Entities.Schedule>().AfterMap((s, d) =>
            {
                d = s.SharedViewModel.MapPropertiesToInstance(d);
            });
            Mapper.CreateMap<DashboardScheduleShareViewModel, Framework.DomainModel.Entities.Schedule>()
                .ForMember(d => d.StartTime, opt => opt.Ignore())
                .ForMember(d => d.EndTime, opt => opt.Ignore())
                 .ForMember(d => d.DurationStart, opt => opt.Ignore())
                .ForMember(d => d.DurationEnd, opt => opt.Ignore())
                .ForMember(d => d.TimeZone, opt => opt.Ignore())
                .AfterMap((s, d) =>
                {

                    d.StartTime = s.StartTime.GetValueOrDefault();
                    d.EndTime = s.EndTime.GetValueOrDefault();

                    d.DurationStart = s.DurationStart.GetValueOrDefault();
                    if (s.IsNoDurationEnd == true)
                    {
                        d.DurationEnd = null;
                    }
                    else
                    {
                        d.DurationEnd = s.DurationEnd.GetValueOrDefault();
                    }
                   
                    //if (s.Id == 0)
                    //{
                    //    d.TimeZone = s.ExpiredTime;
                    //}
                });
        }
    }
}