using System;
using System.Collections.Generic;

namespace Framework.DomainModel.Entities
{
    public class ServiceConfiguration : Entity
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
    }
}
