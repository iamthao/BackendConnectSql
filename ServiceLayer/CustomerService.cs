using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.Utility;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class CustomerService : MasterFileService<Customer>, ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerService(ITenantPersistenceService tenantPersistenceService, ICustomerRepository customerRepository, IBusinessRuleSet<Customer> businessRuleSet = null)
            : base(customerRepository, customerRepository, tenantPersistenceService, businessRuleSet)
        {
            _customerRepository = customerRepository;
        }

    }
}