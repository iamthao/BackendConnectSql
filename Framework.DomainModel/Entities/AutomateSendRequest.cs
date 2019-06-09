using System;
using System.Collections.Generic;

namespace Framework.DomainModel.Entities
{
    public class AutomateSendRequest : Entity
    {
        public string CronTrigger { get; set; }
        public string QuartzName { get; set; }
        public virtual Request Request { get; set; }
    }
}
