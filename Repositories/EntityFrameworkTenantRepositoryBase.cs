using System;
using Database.Persistance.Tenants;
using Framework.DomainModel;
using Framework.DomainModel.ValueObject;
using Framework.Utility;

namespace Repositories
{
    public class EntityFrameworkTenantRepositoryBase<TEntity> :
        EntityFrameworkRepositoryBase<ITenantPersistenceService, ITenantWorkspace, TEntity>
        where TEntity : Entity
    {
        public virtual ITenantPersistenceService TenantPersistenceService { get; protected set; }

        public EntityFrameworkTenantRepositoryBase(ITenantPersistenceService persistenceService)
            : base(persistenceService, x => x.Context)
        {
            TenantPersistenceService = persistenceService;
        }

        public override void ChangeConnectionString(string connectionString)
        {
            TenantPersistenceService.CurrentWorkspace.Context.Database.Connection.ConnectionString = connectionString;
        }


        public string GetConnectionString()
        {
            return TenantPersistenceService.CurrentWorkspace.Context.Database.Connection.ConnectionString;
        }
        public override bool CheckConnectionString(string connectionString)
        {
            var context = new TenantDataContext(connectionString);
            return context.Database.Exists();
        }
    }
}