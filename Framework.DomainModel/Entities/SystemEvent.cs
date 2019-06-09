using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Framework.DomainModel.Entities
{
    public class SystemEvent : Entity
    {
        public string Description { get; set; }
        public int? EventType { get; set; }
        
    }
}
