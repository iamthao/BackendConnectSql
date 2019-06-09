using AutoMapper;
using Framework.Mapping;
using Framework.Utility;
using QuickspatchWeb.Models.Contact;
using QuickspatchWeb.Models.UserRole;
namespace QuickspatchWeb.Models.Mapping
{
    public class ContactMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Framework.DomainModel.Entities.Contact, DashboardContactDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardContactShareViewModel>();
            });
            Mapper.CreateMap<Framework.DomainModel.Entities.Contact, DashboardContactShareViewModel>()
            .ForMember(d => d.Phone, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                d.Phone = s.Phone.ApplyFormatPhone();
            });
            Mapper.CreateMap<DashboardContactDataViewModel, Framework.DomainModel.Entities.Contact>().AfterMap((s, d) =>
            {
                d = s.SharedViewModel.MapPropertiesToInstance(d);
            });
            Mapper.CreateMap<DashboardContactShareViewModel, Framework.DomainModel.Entities.Contact>()
            .ForMember(d => d.Phone, opt => opt.Ignore())
            .AfterMap((s, d) =>
            {
                d.Phone = s.Phone.RemoveFormat();
            });
        }
    }
}