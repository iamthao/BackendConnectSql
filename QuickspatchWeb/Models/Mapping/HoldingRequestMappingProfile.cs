using AutoMapper;
using Framework.Mapping;
using QuickspatchWeb.Models.HoldingRequest;

namespace QuickspatchWeb.Models.Mapping
{
    public class HoldingRequestMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Framework.DomainModel.Entities.HoldingRequest, DashboardHoldingRequestDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardHoldingRequestShareViewModel>();
            });
            Mapper.CreateMap<Framework.DomainModel.Entities.HoldingRequest, DashboardHoldingRequestShareViewModel>();

            Mapper.CreateMap<DashboardHoldingRequestDataViewModel, Framework.DomainModel.Entities.HoldingRequest>().AfterMap((s, d) =>
            {
                d = s.SharedViewModel.MapPropertiesToInstance(d);
            });
            Mapper.CreateMap<DashboardHoldingRequestShareViewModel, Framework.DomainModel.Entities.HoldingRequest>();
        }
    }
}