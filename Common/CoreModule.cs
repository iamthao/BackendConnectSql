using Autofac;
using Autofac.Extras.DynamicProxy2;
//using Database.Persistance.Deployments;
using Database.Persistance.Tenants;
using Framework.Exceptions.DataAccess.Interceptor;
using Framework.Exceptions.DataAccess.Meta;
using Framework.Exceptions.DataAccess.Translator;
using Framework.Service.Diagnostics;
using Framework.Web;
using Repositories;
using Repositories.Interfaces;
using ServiceLayer.Authentication;
using ServiceLayer.Authorization;
using ServiceLayer.Interfaces.Authentication;
using System.Web.SessionState;

namespace Common
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            RegisterServices(builder);
            RegisterRepositories(builder);
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<EntityFrameworkTenantPersistenceService>().As<ITenantPersistenceService>().InstancePerLifetimeScope();
            builder.RegisterType<DiagnosticService>().As<IDiagnosticService>();
            builder.RegisterType<QuickspatchHttpContext>().As<IQuickspatchHttpContext>().InstancePerLifetimeScope();
            builder.RegisterType<SessionIDManager>().As<ISessionIDManager>().InstancePerLifetimeScope();
            builder.RegisterType<AuthenticationService>().As<IAuthenticationService>().InstancePerLifetimeScope();
            builder.RegisterType<ClaimsManager>().As<IClaimsManager>().InstancePerLifetimeScope();
            builder.RegisterType<OperationAuthorization>().As<IOperationAuthorization>();
            builder.RegisterType<FormAuthenticationService>().As<IFormAuthenticationService>();
            
        }

        private void RegisterRepositoriesInterceptor(ContainerBuilder builder)
        {
            builder.RegisterType<DataAccessExceptionInterceptor>();
        }

        private void RegisterTenantSystemRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<SqlServerDbMetaInfo>().As<IDbMetaInfo>();
            builder.RegisterType<EntityFrameworkExceptionTranslator>().As<IExceptionTranslator>();
        }

        private void RegisterRepositories(ContainerBuilder builder)
        {
            RegisterRepositoriesInterceptor(builder);
            RegisterTenantSystemRepositories(builder);
            RegisterEntityFrameworkRepositories(builder);
        }

        private void RegisterEntityFrameworkRepositories(ContainerBuilder builder)
        {
            builder.RegisterType<EntityFrameworkUserRepository>().As<IUserRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkUserRoleFunctionRepository>().As<IUserRoleFunctionRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkGridConfigRepository>().As<IGridConfigRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkUserRoleRepository>().As<IUserRoleRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkUserRoleFunctionRepository>().As<IUserRoleFunctionRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkModuleRepository>().As<IModuleRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkModuleDocumentTypeOperationRepository>().As<IModuleDocumentTypeOperationRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkFranchiseeTenantRepository>().As<IFranchiseeTenantRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkFranchiseeModuleRepository>().As<IFranchiseeModuleRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkDocumentTypeRepository>().As<IDocumentTypeRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkRequestRepository>().As<IRequestRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkRequestHistoryRepository>().As<IRequestHistoryRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkHoldingRequestRepository>().As<IHoldingRequestRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkCustomerRepository>().As<ICustomerRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkLocationRepository>().As<ILocationRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkStateRepository>().As<IStateRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkCourierRepository>().As<ICourierRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkTrackingRepository>().As<ITrackingRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkStaticValueRepository>().As<IStaticValueRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkScheduleRepository>().As<IScheduleRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkNoteRequestReporsitory>().As<INoteRequestRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkFranchiseeConfigurationRepository>().As<IFranchiseeConfigurationRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkCountryOrRegionRepository>().As<ICountryOrRegionRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkSystemEventRepository>().As<ISystemEventRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkConfigFranchiseeRepository>().As<IConfigFranchiseeRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkPackageHistoryRepository>().As<IPackageHistoryRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkRegisterRepository>().As<IRegisterRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkContactRepository>().As<IContactRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkTemplateRepository>().As<ITemplateRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkSystemConfigurationRepository>().As<ISystemConfigurationRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));
            builder.RegisterType<EntityFrameworkTableVersionRepository>().As<ITableVersionRepository>().EnableInterfaceInterceptors().InterceptedBy(typeof(DataAccessExceptionInterceptor));                 
  
        }
    }
}
