using System;
using System.Security.Cryptography;
using AutoMapper;
using Framework.Mapping;
using Framework.Utility;
using QuickspatchWeb.Models.Location;
using QuickspatchWeb.Models.Template;

namespace QuickspatchWeb.Models.Mapping
{
    public class TemplateMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Framework.DomainModel.Entities.Template, DashboardTemplateDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardTemplateShareViewModel>();
            });
            Mapper.CreateMap<Framework.DomainModel.Entities.Template, DashboardTemplateShareViewModel>();

            Mapper.CreateMap<DashboardTemplateDataViewModel, Framework.DomainModel.Entities.Template>().AfterMap((s, d) =>
            {
                d = s.SharedViewModel.MapPropertiesToInstance(d);
            });
            Mapper.CreateMap<DashboardTemplateShareViewModel, Framework.DomainModel.Entities.Template>();


        }
    }
}