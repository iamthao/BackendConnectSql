using AutoMapper;
using Framework.Mapping;
using QuickspatchWeb.Models.ModuleDocumentTypeOperation;

namespace QuickspatchWeb.Models.Mapping
{
    public class ModuleDocumentTypeOperationMappingProfile : Profile
    {
        protected override void Configure()
        {
            Mapper.CreateMap<Framework.DomainModel.Entities.ModuleDocumentTypeOperation, DashboardModuleDocumentTypeOperationDataViewModel>().AfterMap((s, d) =>
            {
                d.SharedViewModel = s.MapTo<DashboardModuleDocumentTypeOperationShareViewModel>();
            });
            Mapper.CreateMap<Framework.DomainModel.Entities.ModuleDocumentTypeOperation, DashboardModuleDocumentTypeOperationShareViewModel>();

            Mapper.CreateMap<DashboardModuleDocumentTypeOperationDataViewModel, Framework.DomainModel.Entities.ModuleDocumentTypeOperation>().AfterMap((s, d) =>
            {
                d = s.SharedViewModel.MapPropertiesToInstance(d);
            });
            Mapper.CreateMap<DashboardModuleDocumentTypeOperationShareViewModel, Framework.DomainModel.Entities.ModuleDocumentTypeOperation>();
        }
    }
}