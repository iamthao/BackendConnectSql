using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Framework.DomainModel.Entities
{
    public enum TableInfo
    {
        Location = 1,
        Courier = 2,
    }
    public class TableVersion : Entity
    {
        public TableVersion()
        {
        }

        public TableInfo TableId { get; set; }
        public string Version { get; set; }
    }
}
