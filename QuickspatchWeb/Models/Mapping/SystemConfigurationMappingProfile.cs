using System;
using System.Security.Cryptography;
using AutoMapper;
using Framework.Mapping;
using Framework.Utility;
using QuickspatchWeb.Models.SystemConfiguration;

namespace QuickspatchWeb.Models.Mapping
{
    public class SystemConfigurationMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Framework.DomainModel.Entities.SystemConfiguration, DashboardSystemConfigurationDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardSystemConfigurationShareViewModel>();
            });
            Mapper.CreateMap <Framework.DomainModel.Entities.SystemConfiguration, DashboardSystemConfigurationShareViewModel>();

            Mapper.CreateMap<DashboardSystemConfigurationDataViewModel, Framework.DomainModel.Entities.SystemConfiguration>().AfterMap((s, d) =>
            {
                d = s.SharedViewModel.MapPropertiesToInstance(d);
            });
            Mapper .CreateMap <DashboardSystemConfigurationShareViewModel, Framework.DomainModel.Entities.SystemConfiguration>();

        }
    }
}