
using Framework.DomainModel.ValueObject;
using Framework.Utility;

namespace Database.Persistance.Tenants
{
    public class EntityFrameworkTenantPersistenceService : EntityFrameworkPersistenceServiceBase<ITenantWorkspace>,
        ITenantPersistenceService
    {
        public EntityFrameworkTenantPersistenceService(IDeploymentService deploymentService)
        {
            DeploymentService = deploymentService;
        }

        public IDeploymentService DeploymentService { get; set; }

        public ITenantWorkspace CreateWorkspace(Tenant tenant)
        {
            var workspace = CreateWorkspaceCore(tenant);
            SetWorkspace(workspace);
            return workspace;
        }

        private ITenantWorkspace CreateWorkspaceCore(Tenant tenant)
        {
            var connectionString = PersistenceHelper.GenerateConnectionString(tenant.Server, tenant.Database,tenant.UserName,tenant.Password);
            if (string.IsNullOrEmpty(tenant.Server) || string.IsNullOrEmpty(tenant.Database))
            {
                connectionString = tenant.Name;
            }
            var context = new TenantDataContext(connectionString);
            var workspace = new EntityFrameworkTenantWorkspace(context);
            return workspace;
        }

        protected override ITenantWorkspace CreateWorkspaceCore()
        {
            var tenant = DeploymentService.GetCurrentTenant();
            var workspace = CreateWorkspaceCore(tenant);
            return workspace;
        }
    }
}