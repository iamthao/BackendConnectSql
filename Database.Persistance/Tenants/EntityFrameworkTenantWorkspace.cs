namespace Database.Persistance.Tenants
{
    public class EntityFrameworkTenantWorkspace : EntityFrameworkWorkspaceBase<TenantDataContext>, ITenantWorkspace
    {
        public EntityFrameworkTenantWorkspace(TenantDataContext context)
            : base(context)
        {
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}