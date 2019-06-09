using System;
using Autofac;

namespace Framework.Service.Container
{
    public class AutofacContainerService : IContainerService
    {
        public AutofacContainerService(IComponentContext context)
        {
            Context = context;
        }

        public IComponentContext Context { get; set; }

        public T Resolve<T>()
        {
            return Context.Resolve<T>();
        }

        public object Resolve(Type serviceType)
        {
            return Context.Resolve(serviceType);
        }
    }
}
