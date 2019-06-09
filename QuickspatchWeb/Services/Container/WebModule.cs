using Autofac;
using Autofac.Integration.Mvc;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.Utility;
using QuickspatchWeb.Models;
using QuickspatchWeb.Services.Deployment;
using QuickspatchWeb.Services.Implement;
using QuickspatchWeb.Services.Interface;
using ServiceLayer;
using ServiceLayer.BusinessRules.Common;
using ServiceLayer.BusinessRules.Franchisee;
using ServiceLayer.BusinessRules.FranchiseeConfiguration;
using ServiceLayer.BusinessRules.HoldingRequest;
using ServiceLayer.BusinessRules.Location;
using ServiceLayer.BusinessRules.Module;
using ServiceLayer.BusinessRules.Request;
using ServiceLayer.BusinessRules.Schedule;
using ServiceLayer.BusinessRules.User;
using ServiceLayer.BusinessRules.UserRole;
using ServiceLayer.BusinessRules.Courier;
using ServiceLayer.BusinessRules.Template;
using ServiceLayer.BusinessRules.SystemConfiguration;
using ServiceLayer.Interfaces;
using System;
using System.Linq;
using System.Reflection;
using Consume.ServiceLayer;
using Consume.ServiceLayer.Interface;
using ServiceLayer.BusinessRules.Contact;
using IUserService = ServiceLayer.Interfaces.IUserService;
using Module = Autofac.Module;
using UserService = ServiceLayer.UserService;

namespace QuickspatchWeb.Services.Container
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            RegisterAspNetDependencies(builder);
            RegisterSystemServices(builder);
            RegisterService(builder);
            RegisterWebService(builder);
            RegisterViewModels(builder);
            RegisterBusinessRules(builder);
        }

        private void RegisterAspNetDependencies(ContainerBuilder builder)
        {
            builder.RegisterControllers(typeof(MvcApplication).Assembly).InstancePerRequest();
        }

        private void RegisterSystemServices(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(MasterFileService<>)).As(typeof(IMasterFileService<>));
        }
        private void RegisterService(ContainerBuilder builder)
        {
            builder.RegisterType<WebDeploymentService>().As<IDeploymentService>().InstancePerLifetimeScope();
        }

        private void RegisterWebService(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<GridConfigService>().As<IGridConfigService>();
            builder.RegisterType<EmailHandler>().As<IEmailHandler>();
            builder.RegisterType<RazorRenderViewToStringService>().As<IRenderViewToString>();
            builder.RegisterType<ConfigurationReader>().As<IConfigurationReader>();
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
            builder.RegisterType<SystemPrintPdfService>().As<ISystemPrintPdfService>();
            builder.RegisterType<RegisterService>().As<IRegisterService>();
            builder.RegisterType<PackageHistoryService>().As<IPackageHistoryService>();
            builder.RegisterType<WebApiPaymentService>().As<IWebApiPaymentService>();
            builder.RegisterType<ContactService>().As<IContactService>();
            builder.RegisterType<TemplateService>().As<ITemplateService>();
            builder.RegisterType<SystemConfigurationService>().As<ISystemConfigurationService>();
            builder.RegisterType<TableVersionService>().As<ITableVersionService>();
        }

        private void RegisterBusinessRules(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(DataAnnotationBusinessRule<>)).As(typeof(IBusinessRule<>));

            builder.RegisterType<BusinessRuleSet<User>>().AsImplementedInterfaces();
            builder.RegisterType<UserRule<User>>().AsImplementedInterfaces();

            builder.RegisterType<BusinessRuleSet<UserRole>>().AsImplementedInterfaces();
            builder.RegisterType<UserRoleRule<UserRole>>().AsImplementedInterfaces();

            builder.RegisterType<BusinessRuleSet<Framework.DomainModel.Entities.Module>>().AsImplementedInterfaces();
            builder.RegisterType<ModuleRule<Framework.DomainModel.Entities.Module>>().AsImplementedInterfaces();

            builder.RegisterType<BusinessRuleSet<FranchiseeTenant>>().AsImplementedInterfaces();
            builder.RegisterType<FranchiseeRule<FranchiseeTenant>>().AsImplementedInterfaces();

            //location
            builder.RegisterType<BusinessRuleSet<Location>>().AsImplementedInterfaces();
            builder.RegisterType<LocationRule<Location>>().AsImplementedInterfaces();

            //Courier
            builder.RegisterType<BusinessRuleSet<Courier>>().AsImplementedInterfaces();
            builder.RegisterType<CourierRule<Courier>>().AsImplementedInterfaces();

            builder.RegisterType<BusinessRuleSet<HoldingRequest>>().AsImplementedInterfaces();
            builder.RegisterType<HoldingRequestRule<HoldingRequest>>().AsImplementedInterfaces();

            builder.RegisterType<BusinessRuleSet<Request>>().AsImplementedInterfaces();
            builder.RegisterType<RequestRule<Request>>().AsImplementedInterfaces();

            builder.RegisterType<BusinessRuleSet<Schedule>>().AsImplementedInterfaces();
            builder.RegisterType<ScheduleRule<Schedule>>().AsImplementedInterfaces();


            builder.RegisterType<BusinessRuleSet<FranchiseeConfiguration>>().AsImplementedInterfaces();
            builder.RegisterType<FranchiseeConfigurationRule<FranchiseeConfiguration>>().AsImplementedInterfaces();

            builder.RegisterType<BusinessRuleSet<Contact>>().AsImplementedInterfaces();
            builder.RegisterType<ContactRule<Contact>>().AsImplementedInterfaces();

            builder.RegisterType<BusinessRuleSet<Template>>().AsImplementedInterfaces();
            builder.RegisterType<TemplateRule<Template>>().AsImplementedInterfaces();

            builder.RegisterType<BusinessRuleSet<SystemConfiguration>>().AsImplementedInterfaces();
            builder.RegisterType<SystemConfigurationRule<SystemConfiguration>>().AsImplementedInterfaces();
                       
        }

        private void RegisterViewModels(ContainerBuilder builder)
        {
            RegisterInstanceOfType(builder, ThisAssembly, typeof(ViewModelBase));
        }

        private static void RegisterInstanceOfType(ContainerBuilder builder, Assembly assembly, Type typeRegistering)
        {
            var types = assembly.GetTypes();
            foreach (var type in types.Where(typeRegistering.IsAssignableFrom))
            {
                builder.RegisterType(type).AsSelf();
            }
        }
    }
}
