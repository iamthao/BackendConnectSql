using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.Utility;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using System.Configuration;
using System.Transactions;
using Framework.DomainModel.DataTransferObject;

namespace ServiceLayer
{
    public class RegisterService : MasterFileService<Register>, IRegisterService
    {
        private readonly IRegisterRepository _registerRepository;
        public RegisterService(ITenantPersistenceService tenantPersistenceService, IRegisterRepository registerRepository, IBusinessRuleSet<Register> businessRuleSet = null)
            : base(registerRepository, registerRepository, tenantPersistenceService, businessRuleSet)
        {
            _registerRepository = registerRepository;
        }
        
    }
}