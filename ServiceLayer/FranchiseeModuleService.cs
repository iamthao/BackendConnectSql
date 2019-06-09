using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class FranchiseeModuleService : MasterFileService<FranchiseeModule>, IFranchiseeModuleService
    {
        private readonly IFranchiseeModuleRepository _franchiseeModuleRepository;
        public FranchiseeModuleService(ITenantPersistenceService tenantPersistenceService, IFranchiseeModuleRepository franchiseeModuleRepository,
            IBusinessRuleSet<FranchiseeModule> businessRuleSet = null)
            : base(franchiseeModuleRepository, franchiseeModuleRepository, tenantPersistenceService, businessRuleSet)
        {
            _franchiseeModuleRepository = franchiseeModuleRepository;
        }
    }
}