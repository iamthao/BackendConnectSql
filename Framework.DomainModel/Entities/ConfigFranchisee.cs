using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Framework.DomainModel.Entities
{
    public enum ConfigType
    {
        String = 1,
        Int = 2,
        Json = 3,

    }
    public class ConfigFranchisee : Entity
    {
        public ConfigFranchisee()
        {
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public ConfigType ConfigType { get; set; }
    }
}
