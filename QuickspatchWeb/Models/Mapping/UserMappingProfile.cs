using AutoMapper;
using Framework.Mapping;
using Framework.Utility;
using QuickspatchWeb.Models.User;
using System;

namespace QuickspatchWeb.Models.Mapping
{
    public class UserMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Framework.DomainModel.Entities.User, DashboardUserDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardUserShareViewModel>();
            });
            Mapper.CreateMap<Framework.DomainModel.Entities.User, DashboardUserShareViewModel>()
                .ForMember(d => d.HomePhone, opt => opt.Ignore())
                .ForMember(d => d.MobilePhone, opt => opt.Ignore())
                .ForMember(d => d.Avatar, opt => opt.Ignore())
                .AfterMap((s, d) =>
                {
                    d.HomePhone = s.HomePhone.ApplyFormatPhone();
                    d.MobilePhone = s.MobilePhone.ApplyFormatPhone();
                    if (s.Avatar != null)
                    {
                        d.Avatar = "data:image/jpg;base64," + Convert.ToBase64String(s.Avatar);
                    }
                });

            Mapper.CreateMap<DashboardUserDataViewModel, Framework.DomainModel.Entities.User>().AfterMap((s, d) =>
            {
                d = s.SharedViewModel.MapPropertiesToInstance(d);
            });
            Mapper.CreateMap<DashboardUserShareViewModel, Framework.DomainModel.Entities.User>()
                .ForMember(d => d.HomePhone, opt => opt.Ignore())
                .ForMember(d => d.MobilePhone, opt => opt.Ignore())
                .ForMember(o => o.Avatar, opt => opt.Ignore())
                .AfterMap((s, d) =>
                {
                    d.HomePhone = s.HomePhone.RemoveFormat();
                    d.MobilePhone = s.MobilePhone.RemoveFormat();
                    
                });
        }
    }
}