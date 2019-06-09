using System;
using AutoMapper;
using Framework.Mapping;
using Framework.Utility;
using QuickspatchWeb.Models.FranchiseeTenant;

namespace QuickspatchWeb.Models.Mapping
{
    public class FranchiseeTenantMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Framework.DomainModel.Entities.FranchiseeTenant, DashboardFranchiseeTenantDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardFranchiseeTenantShareViewModel>();
            });
            Mapper.CreateMap<Framework.DomainModel.Entities.FranchiseeTenant, DashboardFranchiseeTenantShareViewModel>();

            Mapper.CreateMap<DashboardFranchiseeTenantDataViewModel, Framework.DomainModel.Entities.FranchiseeTenant>().AfterMap((s, d) =>
            {
                d = s.SharedViewModel.MapPropertiesToInstance(d);
            });
            Mapper.CreateMap<DashboardFranchiseeTenantShareViewModel, Framework.DomainModel.Entities.FranchiseeTenant>()
                .ForMember(desc => desc.FaxNumber, opt => opt.Ignore())
                .ForMember(desc => desc.OfficePhone, opt => opt.Ignore())
                .ForMember(d => d.PrimaryContactCellNumber, opt => opt.Ignore())//
                .ForMember(d => d.PrimaryContactFax, opt => opt.Ignore())//
                .ForMember(d => d.PrimaryContactPhone, opt => opt.Ignore())//
                .AfterMap(
                (s, d) =>
                {
                    d.PrimaryContactCellNumber = s.PrimaryContactCellNumber.RemoveFormatPhone();//
                    d.PrimaryContactFax = s.PrimaryContactFax.RemoveFormatPhone();//
                    d.PrimaryContactPhone = s.PrimaryContactPhone.RemoveFormatPhone();//
                    d.FaxNumber = s.FaxNumber.RemoveFormat();
                    d.OfficePhone = s.OfficePhone.RemoveFormat();
                });
            
            Mapper.CreateMap<DashboardFranchiseeTenantDataViewModel, Framework.DomainModel.Entities.FranchiseeConfiguration>().AfterMap((s, d) =>
            {
                d = s.SharedViewModel.MapPropertiesToInstance(d);
            });

            Mapper.CreateMap <DashboardFranchiseeTenantShareViewModel, Framework.DomainModel.Entities.FranchiseeConfiguration>()
            .ForMember(d => d.Logo, opt => opt.Ignore())
            .ForMember(d => d.PrimaryContactCellNumber, opt => opt.Ignore())//
                .ForMember(d => d.PrimaryContactFax, opt => opt.Ignore())//
                .ForMember(d => d.PrimaryContactPhone, opt => opt.Ignore())//
            .AfterMap((s, d) =>
            {
                d.PrimaryContactCellNumber = s.PrimaryContactCellNumber.RemoveFormatPhone();//
                d.PrimaryContactFax = s.PrimaryContactFax.RemoveFormatPhone();//
                d.PrimaryContactPhone = s.PrimaryContactPhone.RemoveFormatPhone();//
                if (s.Logo != null)
                {
                    if (s.Logo.Contains("data:image/jpg;base64,"))
                    {
                        d.Logo = Convert.FromBase64String(s.Logo.Replace("data:image/jpg;base64,", ""));
                    }
                }
                else
                {
                    d.Logo = null;
                }

            });


            Mapper.CreateMap<Framework.DomainModel.Entities.FranchiseeTenant, DashboardFranchiseeTenantDataViewModel>().AfterMap((s, d) =>
            {
                if (d.SharedViewModel != null)
                {
                    s.MapPropertiesToInstance(d.SharedViewModel);
                }
                else
                {
                    d.SharedViewModel = s.MapTo<DashboardFranchiseeTenantShareViewModel>();
                }

            });

            Mapper.CreateMap<Framework.DomainModel.Entities.FranchiseeTenant, DashboardFranchiseeTenantShareViewModel>()
                .ForMember(desc => desc.FaxNumber, opt => opt.Ignore())
                .ForMember(desc => desc.OfficePhone, opt => opt.Ignore())
                
                .AfterMap(
                (s, d) =>
                {
                   
                    d.FaxNumber = s.FaxNumber.ApplyFormatPhone();
                    d.OfficePhone = s.OfficePhone.ApplyFormatPhone();
                });

            Mapper.CreateMap<Framework.DomainModel.Entities.FranchiseeConfiguration, DashboardFranchiseeTenantDataViewModel>().AfterMap((s, d) =>
            {
                if (d.SharedViewModel != null)
                {
                    s.MapPropertiesToInstance(d.SharedViewModel);
                }
                else
                {
                    d.SharedViewModel = s.MapTo<DashboardFranchiseeTenantShareViewModel>();
                }
            });

            Mapper.CreateMap<Framework.DomainModel.Entities.FranchiseeConfiguration, DashboardFranchiseeTenantShareViewModel>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(desc => desc.FaxNumber, opt => opt.Ignore())
                .ForMember(desc => desc.OfficePhone, opt => opt.Ignore())
                .ForMember(d => d.PrimaryContactCellNumber, opt => opt.Ignore())//
                .ForMember(d => d.PrimaryContactFax, opt => opt.Ignore())//
                .ForMember(d => d.PrimaryContactPhone, opt => opt.Ignore())//
                .ForMember(d => d.Logo, opt => opt.Ignore())
                .AfterMap((s, d) =>
                {
                    d.PrimaryContactCellNumber = s.PrimaryContactCellNumber.ApplyFormatPhone();//
                    d.PrimaryContactFax = s.PrimaryContactFax.ApplyFormatPhone();//
                    d.PrimaryContactPhone = s.PrimaryContactPhone.ApplyFormatPhone();//
                    d.FaxNumber = s.FaxNumber.ApplyFormatPhone();
                    d.OfficePhone = s.OfficePhone.ApplyFormatPhone();
                    if (s.Logo != null)
                    {
                        d.Logo = "data:image/jpg;base64," + Convert.ToBase64String(s.Logo);
                    }
                });

        }
    }
}