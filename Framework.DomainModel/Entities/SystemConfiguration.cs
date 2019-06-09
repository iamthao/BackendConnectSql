using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Framework.DomainModel.Entities
{
    public class SystemConfiguration : Entity
    {
        public SystemConfiguration()
        {
        }

        public string Name { get; set; }
        public string Value { get; set; }
        public DataType DataType { get; set; }
        public SystemConfigType SystemConfigType { get; set; }
    }

    public enum DataType 
    {     
        [Description("String")]
        String = 1,
        [Description("Int")]
        Int = 2,
        [Description("Double")]
        Double = 3,
        [Description("Float")]
        Float = 4, 
        [Description("DateTime")]
        DateTime = 5, 
    }

    public enum SystemConfigType 
    {
        [Description("DispatchTimeDefault")]
        DispatchTimeDefault = 1,
        [Description("RequestNo")]
        RequestNo = 2,
        [Description("DefaultLocationFrom")]
        DefaultLocationFrom = 3,
        [Description("DefaultLocationTo")]
        DefaultLocationTo = 4,
        
    }
}
