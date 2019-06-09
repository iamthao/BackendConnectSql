using System.Transactions;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Framework.Utility;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class FranchiseeConfigurationService : MasterFileService<FranchiseeConfiguration>, IFranchiseeConfigurationService
    {
        private readonly IFranchiseeConfigurationRepository _franchiseeConfigurationRepository;
        private readonly IFranchiseeTenantRepository _franchiseeTenantRepository;
        private readonly IFranchiseeTenantService _franchiseeTenantService;
        public FranchiseeConfigurationService(ITenantPersistenceService tenantPersistenceService, 
            IFranchiseeConfigurationRepository franchiseeConfigurationRepository,IFranchiseeTenantRepository franchiseeTenantRepository,IFranchiseeTenantService franchiseeTenantService,
            IBusinessRuleSet<FranchiseeConfiguration> businessRuleSet = null)
            : base(franchiseeConfigurationRepository, franchiseeConfigurationRepository, tenantPersistenceService, businessRuleSet)
        {
            _franchiseeConfigurationRepository = franchiseeConfigurationRepository;
            _franchiseeTenantRepository = franchiseeTenantRepository;
            _franchiseeTenantService = franchiseeTenantService;
        }

        public FranchiseeConfiguration GetFranchiseeConfiguration()
        {
            return _franchiseeConfigurationRepository.GetFranchiseeConfiguration();
        }

        public void ValidateBusinessRulesFranchiseeConfig(FranchiseeConfiguration franchiseeConfiguration)
        {
            ValidateBusinessRules(franchiseeConfiguration);
        }

    }
}