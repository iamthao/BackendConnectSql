using Autofac;
using Consume.ServiceLayer;
using Consume.ServiceLayer.Interface;

namespace QuickspatchWeb.Services.Container
{
    public class WebApiConsumeModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            RegisterSystemServices(builder);
            RegisterWebService(builder);
        }


        private void RegisterSystemServices(ContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(WebApiConsumeMasterFileService<>)).As(typeof(IWebApiConsumeMasterFileService<>));
        }

        private void RegisterWebService(ContainerBuilder builder)
        {
            builder.RegisterType<WebApiConsumeUserService>().As<IWebApiConsumeUserService>(); 
        }
    }
}