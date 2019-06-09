using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.ValueObject
{
    public class WarningScheduleVo
    {
        public string PreviousName { get; set; }
        public DateTime PreviousStartTime { get; set; }
        public DateTime PreviousEndTime { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string NextName { get; set; }
        public DateTime NextStartTime { get; set; }
        public DateTime NextEndTime { get; set; }
        public bool IsUpdate { get; set; }
    }

    
}
