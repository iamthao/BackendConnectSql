using AutoMapper;
using System;
using System.Threading;

namespace ServiceLayer.Mapping
{
    public class AutoMapperProfileThread
    {
        private readonly ManualResetEvent _doneEvent;
        public Profile Profile { get; set; }

        public AutoMapperProfileThread(ManualResetEvent doneEvent)
        {
            _doneEvent = doneEvent;
        }
        public void ThreadPoolCallback(Object threadContext)
        {
            Mapper.AddProfile(Profile);
            _doneEvent.Set();
        }
    }
}