using AutoMapper;
using Framework.Mapping;
using QuickspatchWeb.Models.Module;

namespace QuickspatchWeb.Models.Mapping
{
    public class ModuleMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Framework.DomainModel.Entities.Module, DashboardModuleDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardModuleShareViewModel>();
            });
            Mapper.CreateMap<Framework.DomainModel.Entities.Module, DashboardModuleShareViewModel>();

            Mapper.CreateMap<DashboardModuleDataViewModel, Framework.DomainModel.Entities.Module>().AfterMap((s, d) =>
            {
                d = s.SharedViewModel.MapPropertiesToInstance(d);
            });
            Mapper.CreateMap<DashboardModuleShareViewModel, Framework.DomainModel.Entities.Module>();
        }
    }
}