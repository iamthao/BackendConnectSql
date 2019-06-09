using AutoMapper;
using Framework.Mapping;
using QuickspatchWeb.Models.NoteRequest;
using QuickspatchWeb.Models.Request;

namespace QuickspatchWeb.Models.Mapping
{
    public class RequestMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Framework.DomainModel.Entities.Request, DashboardRequestDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardRequestShareViewModel>();
            });
            Mapper.CreateMap<Framework.DomainModel.Entities.Request, DashboardRequestShareViewModel>()
            .AfterMap((s, d) =>
            {
                
            });

            Mapper.CreateMap<DashboardRequestDataViewModel, Framework.DomainModel.Entities.Request>().AfterMap((s, d) =>
            {
                d = s.SharedViewModel.MapPropertiesToInstance(d);
            });
            Mapper.CreateMap<DashboardRequestShareViewModel, Framework.DomainModel.Entities.Request>().AfterMap(
                (s, d) =>
                {
                    d.CourierId = s.CourierId;
                });
        }
    }
}