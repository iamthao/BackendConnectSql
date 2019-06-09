using System.Linq;
using AutoMapper;
using Framework.DomainModel.Common;
using Framework.DomainModel.Entities;
using Framework.Mapping;
using Framework.Utility;
using QuickspatchWeb.Models.Courier;
using QuickspatchWeb.Models.User;

namespace QuickspatchWeb.Models.Mapping
{
    public class CourierMappingProfile : Profile
    {
        protected override void Configure()
        {
            //
            Mapper.CreateMap<Framework.DomainModel.Entities.Courier, DashboardCourierDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardCourierShareViewModel>();
            });
            Mapper.CreateMap<Framework.DomainModel.Entities.Courier, DashboardCourierShareViewModel>()
            .AfterMap((s, d) =>
            {
                d.UserShareViewModel = s.User.MapTo<DashboardUserShareViewModel>();
            });
            
            //
            Mapper.CreateMap<DashboardCourierDataViewModel, Framework.DomainModel.Entities.Courier>()
            .AfterMap((s, d) => s.SharedViewModel.MapPropertiesToInstance(d));
            Mapper.CreateMap<DashboardCourierShareViewModel, Framework.DomainModel.Entities.Courier>()
            .AfterMap((s, d) =>
            {
                if (d.User == null)
                    {
                        d.User = new Framework.DomainModel.Entities.User();
          
                    }
                
                s.UserShareViewModel.MapPropertiesToInstance(d.User);

            });
            
        }

    }          
}