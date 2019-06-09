using Autofac;
using Database.Persistance.Tenants;
using Framework.Utility;
using QuickSpatchWindowsService.Common;
using ServiceLayer;
using ServiceLayer.Interfaces;

namespace QuickSpatchWindowsService
{
    public class ApplicationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            RegisterSystemServices(builder);
            RegisterServices(builder);
        }

        private void RegisterSystemServices(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(MasterFileService<>)).As(typeof(IMasterFileService<>));
            builder.RegisterType<ServiceDeploymentService>().As<IDeploymentService>().InstancePerLifetimeScope();
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<GridConfigService>().As<IGridConfigService>();
            builder.RegisterType<EmailHandler>().As<IEmailHandler>();
            builder.RegisterType<UserRoleService>().As<IUserRoleService>();
            builder.RegisterType<ResizeImageService>().As<IResizeImage>();
            builder.RegisterType<TempUploadFileService>().As<ITempUploadFileService>();
            builder.RegisterType<ModuleService>().As<IModuleService>();
            builder.RegisterType<FranchiseeModuleService>().As<IFranchiseeModuleService>();
            builder.RegisterType<FranchiseeTenantService>().As<IFranchiseeTenantService>();
            builder.RegisterType<ModuleDocumentTypeOperationService>().As<IModuleDocumentTypeOperationService>();
            builder.RegisterType<DocumentTypeService>().As<IDocumentTypeService>();
            builder.RegisterType<CustomerService>().As<ICustomerService>();
            builder.RegisterType<LocationService>().As<ILocationService>();
            builder.RegisterType<TrackingService>().As<ITrackingService>();
            builder.RegisterType<StateService>().As<IStateService>();
            builder.RegisterType<RequestService>().As<IRequestService>();
            builder.RegisterType<HoldingRequestService>().As<IHoldingRequestService>();
            builder.RegisterType<CourierService>().As<ICourierService>();
            builder.RegisterType<StaticValueService>().As<IStaticValueService>();
            builder.RegisterType<ScheduleService>().As<IScheduleService>();
            builder.RegisterType<NoteRequestService>().As<INoteRequestService>();
            builder.RegisterType<FranchiseeConfigurationService>().As<IFranchiseeConfigurationService>();
            builder.RegisterType<CountryOrRegionService>().As<ICountryOrRegionService>();//
            builder.RegisterType<SystemEventService>().As<ISystemEventService>();
            builder.RegisterType<GoogleService>().As<IGoogleService>();
            builder.RegisterType<RequestHistoryService>().As<IRequestHistoryService>();
            builder.RegisterType<ConfigFranchiseeService>().As<IConfigFranchiseeService>();
            builder.RegisterType<PackageHistoryService>().As<IPackageHistoryService>();
        }
    }
}
