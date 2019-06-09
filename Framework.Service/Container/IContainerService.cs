using System;

namespace Framework.Service.Container
{
    public interface IContainerService
    {
        T Resolve<T>();
        object Resolve(Type serviceType);
    }
}