using AutoMapper;
using Framework.Mapping;
using QuickspatchWeb.Models.UserRole;
namespace QuickspatchWeb.Models.Mapping
{
    public class UserRoleMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Framework.DomainModel.Entities.UserRole, DashboardUserRoleDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardUserRoleShareViewModel>();
            });
            Mapper.CreateMap<Framework.DomainModel.Entities.UserRole, DashboardUserRoleShareViewModel>();

            Mapper.CreateMap<DashboardUserRoleDataViewModel, Framework.DomainModel.Entities.UserRole>().AfterMap((s, d) =>
            {
                d = s.SharedViewModel.MapPropertiesToInstance(d);
            });
            Mapper.CreateMap<DashboardUserRoleShareViewModel, Framework.DomainModel.Entities.UserRole>();
        }
    }
}