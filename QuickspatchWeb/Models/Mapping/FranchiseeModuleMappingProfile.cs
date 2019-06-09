using AutoMapper;
using Framework.Mapping;
using QuickspatchWeb.Models.FranchiseeModule;

namespace QuickspatchWeb.Models.Mapping
{
    public class FranchiseeModuleMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Framework.DomainModel.Entities.FranchiseeModule, DashboardFranchiseeModuleDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardFranchiseeModuleShareViewModel>();
            });
            Mapper.CreateMap<Framework.DomainModel.Entities.FranchiseeModule, DashboardFranchiseeModuleShareViewModel>();

            Mapper.CreateMap<DashboardFranchiseeModuleDataViewModel, Framework.DomainModel.Entities.FranchiseeModule>().AfterMap((s, d) =>
            {
                d = s.SharedViewModel.MapPropertiesToInstance(d);
            });
            Mapper.CreateMap<DashboardFranchiseeModuleShareViewModel, Framework.DomainModel.Entities.FranchiseeModule>();
        }
    }
}