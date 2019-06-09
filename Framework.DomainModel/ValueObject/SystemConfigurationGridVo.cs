using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DomainModel.Common;
using Framework.DomainModel.Entities;
using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class SystemConfigurationGridVo : ReadOnlyGridVo
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public DataType DataType { get; set; }
        public SystemConfigType SystemConfigType { get; set; }
        public int DataTypeId { get; set; }
        public int SystemConfigTypeId { get; set; }
        public string ValueNoFormat { get; set; }
    }
}
