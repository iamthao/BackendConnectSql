using System.Linq;
using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;

namespace Repositories
{
    public class EntityFrameworkTrackingRepository : EntityFrameworkTenantRepositoryBase<Tracking>, ITrackingRepository
    {
        public EntityFrameworkTrackingRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
          
            
        }

    }
}