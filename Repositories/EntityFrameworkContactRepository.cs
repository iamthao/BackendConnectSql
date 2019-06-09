using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Repositories.Interfaces;

namespace Repositories
{
    public class EntityFrameworkContactRepository : EntityFrameworkTenantRepositoryBase<Contact>, IContactRepository
    {
        public EntityFrameworkContactRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("Name");
            DisplayColumnForCombobox = "Name";
        }

        public List<ContactGridVo> GetListContact()
        {
            var query = GetAll().Select(o => new ContactGridVo
            {
                Id = o.Id,
                Name = o.Name,
                Phone = o.Phone
            }).OrderBy(s=>s.Name).ToList();

            return query;
        }
    }
}
