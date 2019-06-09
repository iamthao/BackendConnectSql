using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class ConfigFranchiseeService : MasterFileService<ConfigFranchisee>, IConfigFranchiseeService
    {
        private readonly IConfigFranchiseeRepository _configFranchiseeRepository;
        public ConfigFranchiseeService(ITenantPersistenceService tenantPersistenceService, IConfigFranchiseeRepository configFranchiseeRepository,
            IBusinessRuleSet<ConfigFranchisee> businessRuleSet = null)
            : base(configFranchiseeRepository, configFranchiseeRepository, tenantPersistenceService, businessRuleSet)
        {
            _configFranchiseeRepository = configFranchiseeRepository;
        }
    }
}