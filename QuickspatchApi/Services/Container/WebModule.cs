using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using Autofac;
using Consume.ServiceLayer;
using Consume.ServiceLayer.Interface;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.Utility;
using QuickspatchApi.Services.Deployment;
using ServiceLayer;
using ServiceLayer.Authorization;
using ServiceLayer.BusinessRules.Common;
using ServiceLayer.BusinessRules.User;
using ServiceLayer.BusinessRules.UserRole;
using ServiceLayer.Interfaces;
using Module = Autofac.Module;

namespace QuickspatchApi.Services.Container
{
    public class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            RegisterAspNetDependencies(builder);
            RegisterSystemServices(builder);
            RegisterService(builder);
            RegisterServices(builder);
            RegisterBusinessRules(builder);
        }

        private void RegisterAspNetDependencies(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly()).Where(t => !t.IsAbstract && typeof(ApiController).IsAssignableFrom(t));
        }
        private void RegisterService(ContainerBuilder builder)
        {
            builder.RegisterType<WebDeploymentService>().As<IDeploymentService>().InstancePerLifetimeScope();
        }
        private void RegisterBusinessRules(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(DataAnnotationBusinessRule<>)).As(typeof(IBusinessRule<>));
            builder.RegisterType<BusinessRuleSet<User>>().AsImplementedInterfaces();
            builder.RegisterType<UserRule<User>>().AsImplementedInterfaces();
            builder.RegisterType<BusinessRuleSet<UserRole>>().AsImplementedInterfaces();
            builder.RegisterType<UserRoleRule<UserRole>>().AsImplementedInterfaces();
        }

        private void RegisterSystemServices(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(MasterFileService<>)).As(typeof(IMasterFileService<>));
        }

        private void RegisterServices(ContainerBuilder builder)
        {
            builder.RegisterType<FranchiseeTenantService>().As<IFranchiseeTenantService>();
            builder.RegisterType<UserService>().As<IUserService>();
            builder.RegisterType<CourierService>().As<ICourierService>();
            builder.RegisterType<ContactService>().As<IContactService>();
            builder.RegisterType<RequestService>().As<IRequestService>();
            builder.RegisterType<StaticValueService>().As<IStaticValueService>();
            builder.RegisterType<RequestHistoryService>().As<IRequestHistoryService>();
            builder.RegisterType<TrackingService>().As<ITrackingService>();
            builder.RegisterType<SystemEventService>().As<ISystemEventService>();
            builder.RegisterType<GoogleService>().As<IGoogleService>();
            builder.RegisterType<EmailHandler>().As<IEmailHandler>();
            builder.RegisterType<FranchiseeConfigurationService>().As<IFranchiseeConfigurationService>();

            builder.RegisterType<WebApiConsumeUserService>().As<IWebApiConsumeUserService>();
            builder.RegisterType<RegisterService>().As<IRegisterService>();
            builder.RegisterType<PackageHistoryService>().As<IPackageHistoryService>();
            builder.RegisterType<WebApiPaymentService>().As<IWebApiPaymentService>();
        }
    }
}