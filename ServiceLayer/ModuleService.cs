using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class ModuleService : MasterFileService<Module>, IModuleService
    {
        private readonly IModuleRepository _moduleRepository;
        public ModuleService(ITenantPersistenceService tenantPersistenceService, IModuleRepository moduleRepository,
            IBusinessRuleSet<Module> businessRuleSet = null)
            : base(moduleRepository, moduleRepository, tenantPersistenceService, businessRuleSet)
        {
            _moduleRepository = moduleRepository;
        }
        
        public dynamic GetModuleDocumentTypeOperationsGrid(int moduleId)
        {
            var data = _moduleRepository.GetModuleDocumentTypeOperationsByModuleId(moduleId);
            return new { Data = data, TotalRowCount = data.Count };
        }
    }
}