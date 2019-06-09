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
    public class EntityFrameworkTableVersionRepository : EntityFrameworkTenantRepositoryBase<TableVersion>, ITableVersionRepository
    {
        public EntityFrameworkTableVersionRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
           
        }
    }
}
