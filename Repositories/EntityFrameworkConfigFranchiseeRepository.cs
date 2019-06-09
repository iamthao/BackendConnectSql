using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;

namespace Repositories
{
    public class EntityFrameworkConfigFranchiseeRepository : EntityFrameworkTenantRepositoryBase<ConfigFranchisee>, IConfigFranchiseeRepository
    {
        public EntityFrameworkConfigFranchiseeRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
        }
    } ////
}