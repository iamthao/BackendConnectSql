using System.Collections.Generic;
using System.Linq;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer
{
    public class ContactService : MasterFileService<Contact>,IContactService
    {
        private readonly IContactRepository  _contactRepository;

        public ContactService(ITenantPersistenceService tenantPersistenceService, IContactRepository contactRepository,
            IBusinessRuleSet<Contact> businessRuleSet = null)
            : base(contactRepository, contactRepository, tenantPersistenceService, businessRuleSet)
        {
            _contactRepository = contactRepository;
        }

        public List<ContactGridVo> GetListContact()
        {
            return _contactRepository.GetListContact();
        } 
       
    }
}
