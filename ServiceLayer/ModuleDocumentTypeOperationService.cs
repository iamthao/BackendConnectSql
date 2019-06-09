using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class ModuleDocumentTypeOperationService : MasterFileService<ModuleDocumentTypeOperation>, IModuleDocumentTypeOperationService
    {
        private readonly IModuleDocumentTypeOperationRepository _moduleDocumentTypeOperationRepository;
        public ModuleDocumentTypeOperationService(ITenantPersistenceService tenantPersistenceService, IModuleDocumentTypeOperationRepository moduleDocumentTypeOperationRepository,
            IBusinessRuleSet<ModuleDocumentTypeOperation> businessRuleSet = null)
            : base(moduleDocumentTypeOperationRepository, moduleDocumentTypeOperationRepository, tenantPersistenceService, businessRuleSet)
        {
            _moduleDocumentTypeOperationRepository = moduleDocumentTypeOperationRepository;
        }
    }
}