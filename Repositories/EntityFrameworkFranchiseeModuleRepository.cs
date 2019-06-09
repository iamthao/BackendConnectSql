using Database.Persistance.Tenants;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;

namespace Repositories
{
    public class EntityFrameworkFranchiseeModuleRepository : EntityFrameworkTenantRepositoryBase<FranchiseeModule>, IFranchiseeModuleRepository
    {
        public EntityFrameworkFranchiseeModuleRepository(ITenantPersistenceService persistenceService)
            : base(persistenceService)
        {
            SearchColumns.Add("Name");
            DisplayColumnForCombobox = "Name";
        }
    }
}