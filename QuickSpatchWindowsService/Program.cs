using System;
using System.Configuration.Install;
using System.Reflection;
using System.ServiceProcess;
using Autofac;
using AutoMapper;
using Common;

namespace QuickSpatchWindowsService
{
    internal static class Program
    {
        public static readonly IContainer Container;

        static Program()
        {
            var builder = new ContainerBuilder();
            var coreModule = new CoreModule();
            builder.RegisterModule(coreModule);
            var appModule = new ApplicationModule();
            builder.RegisterModule(appModule);
            Container = builder.Build();
            Mapper.Initialize(x => x.ConstructServicesUsing(Container.Resolve));
        }

        private static void Main(string[] args)
        {

            //Job.RequestStatusJob.RequestStatusThread(new object());
            //Job.SendRequestJob.SendRequestJobThread(new object());
            var servicesToRun = new ServiceBase[]
                {
                    new MainService(), 
                };
            ServiceBase.Run(servicesToRun);
        }
    }
}
