using System.Web.Mvc;
using Common;
using QuickspatchWeb.Services.Container;
using Autofac;
using Autofac.Integration.Mvc;
using AutoMapper;

namespace QuickspatchWeb.App_Start
{
    public class ContainerConfig
    {
        public static void RegisterDependencies()
        {
            var builder = new ContainerBuilder();

            var coreModule = new CoreModule();
            builder.RegisterModule(coreModule);

            var webModule = new WebModule();
            builder.RegisterModule(webModule);

            var webApiModule = new WebApiConsumeModule();
            builder.RegisterModule(webApiModule);

            var container = builder.Build();

            Mapper.Initialize(x => x.ConstructServicesUsing(container.Resolve));

            RegisterMvcResolver(container);
        }

        private static void RegisterMvcResolver(IContainer container)
        {
            var mvcResolver = new AutofacDependencyResolver(container);
            DependencyResolver.SetResolver(mvcResolver);
        }
    }
}