using AutoMapper;
using Framework.Service.StartUp;
using System;
using System.Linq;
using System.Threading;

namespace QuickspatchWeb.Models.Mapping
{
    /// <summary>
    ///     THis is to define the automapper configuration between viewmodel and entity
    ///     This is define once and be initialized once in the application start up.
    /// </summary>
    public class AutoMapperStartupTask : IStartupTask
    {
        public int Order
        {
            get { return 0; }
        }
        public void Execute()
        {
            var types = GetType().Assembly.GetTypes();
            var profileTypes = types.Where(type => typeof(Profile).IsAssignableFrom(type));
            foreach (var type in profileTypes)
            {
                var doneEvent = new ManualResetEvent(false);
                var thread = new AutoMapperProfileThread(doneEvent) { Profile = (Profile)Activator.CreateInstance(type) };
                ThreadPool.QueueUserWorkItem(thread.ThreadPoolCallback);
            }
        }
    }
}