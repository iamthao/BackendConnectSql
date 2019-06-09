using System;
using AutoMapper;
using Framework.Mapping;
using Framework.Utility;
using QuickspatchWeb.Models.FranchiseeConfiguration;
using QuickspatchWeb.Models.Location;

namespace QuickspatchWeb.Models.Mapping
{
    public class FranchiseeConfigurationMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Framework.DomainModel.Entities.FranchiseeConfiguration, DashboardFranchiseeConfigurationDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardFranchiseeConfigurationShareViewModel>();
            });
            Mapper.CreateMap<Framework.DomainModel.Entities.FranchiseeConfiguration, DashboardFranchiseeConfigurationShareViewModel>();

            Mapper.CreateMap<DashboardFranchiseeConfigurationDataViewModel, Framework.DomainModel.Entities.FranchiseeConfiguration>().AfterMap((s, d) =>
            {
                d = s.SharedViewModel.MapPropertiesToInstance(d);
            });
            Mapper.CreateMap<DashboardFranchiseeConfigurationDataViewModel, Framework.DomainModel.DataTransferObject.FranchiseeTernantDto>().AfterMap((s, d) =>
            {
                d = s.SharedViewModel.MapPropertiesToInstance(d);
            });

            Mapper.CreateMap<DashboardFranchiseeConfigurationShareViewModel, Framework.DomainModel.DataTransferObject.FranchiseeTernantDto>();

            Mapper.CreateMap<DashboardFranchiseeConfigurationDataViewModel, Framework.DomainModel.Entities.FranchiseeConfiguration>().AfterMap((s, d) =>
            {
                d = s.SharedViewModel.MapPropertiesToInstance(d);
            });
            Mapper.CreateMap<DashboardFranchiseeConfigurationShareViewModel, Framework.DomainModel.Entities.FranchiseeConfiguration>()
                .ForMember(d => d.FaxNumber, opt => opt.Ignore())
                .ForMember(d => d.OfficePhone, opt => opt.Ignore())
                .ForMember(d => d.PrimaryContactCellNumber, opt => opt.Ignore())//
                .ForMember(d => d.PrimaryContactFax, opt => opt.Ignore())//
                .ForMember(d => d.PrimaryContactPhone, opt => opt.Ignore())//
                .ForMember(d => d.Logo, opt => opt.Ignore())
                .AfterMap(
                (s, d) =>
                {
                    d.FaxNumber = s.FaxNumber.RemoveFormat();
                    d.OfficePhone = s.OfficePhone.RemoveFormat();
                    d.PrimaryContactCellNumber = s.PrimaryContactCellNumber.RemoveFormat();//
                    d.PrimaryContactFax = s.PrimaryContactFax.RemoveFormat();//
                    d.PrimaryContactPhone = s.PrimaryContactPhone.RemoveFormat();//
                    
                });
            Mapper.CreateMap<Framework.DomainModel.Entities.FranchiseeConfiguration, DashboardFranchiseeConfigurationShareViewModel>()
                .ForMember(d => d.FaxNumber, opt => opt.Ignore())
                .ForMember(d => d.OfficePhone, opt => opt.Ignore())
                .ForMember(d => d.Logo, opt => opt.Ignore())
                .ForMember(d => d.PrimaryContactCellNumber, opt => opt.Ignore())//
                .ForMember(d => d.PrimaryContactFax, opt => opt.Ignore())//
                .ForMember(d => d.PrimaryContactPhone, opt => opt.Ignore())//
                .AfterMap((s, d) =>
                {
                    d.FaxNumber = s.FaxNumber.ApplyFormatPhone();
                    d.OfficePhone = s.OfficePhone.ApplyFormatPhone();
                    d.PrimaryContactCellNumber = s.PrimaryContactCellNumber.ApplyFormatPhone();//
                    d.PrimaryContactFax = s.PrimaryContactFax.ApplyFormatPhone();//
                    d.PrimaryContactPhone = s.PrimaryContactPhone.ApplyFormatPhone();//
                    if (s.Logo != null)
                    {
                        d.Logo = "data:image/jpg;base64," + Convert.ToBase64String(s.Logo);
                    }
                });
        }
    }
}