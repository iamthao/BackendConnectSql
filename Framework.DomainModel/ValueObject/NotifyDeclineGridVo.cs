using System;
using System.Globalization;
using Framework.DomainModel.Common;
using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class NotifyDeclineGridVo : ReadOnlyGridVo
    {
        public string Event { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
