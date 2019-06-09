using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper;
using Common;
using QuickspatchApi.Services.Container;

namespace QuickspatchApi
{
    public class ContainerConfig
    {
        public static void RegisterDependencies(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();

            var coreModule = new CoreModule();
            builder.RegisterModule(coreModule);

            var webModule = new WebModule();
            builder.RegisterModule(webModule);

            var container = builder.Build(); 

            Mapper.Initialize(x => x.ConstructServicesUsing(container.Resolve));

            RegisterMvcResolver(container, config);
        }

        private static void RegisterMvcResolver(IContainer container, HttpConfiguration config)
        {
            var mvcResolver = new AutofacWebApiDependencyResolver(container);
            config.DependencyResolver = mvcResolver;
        }
    }
}