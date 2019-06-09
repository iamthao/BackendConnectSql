using System;
using System.Collections.Generic;

namespace Framework.DomainModel.Entities
{
    public class HoldingRequest : Entity
    {
        public int LocationFrom { get; set; }
        public int LocationTo { get; set; }
        public string Description { get; set; }
        public DateTime? SendDate { get; set; }
        public System.DateTime? StartTime { get; set; }
        public System.DateTime? EndTime { get; set; }
        public virtual Location Location { get; set; }
        public virtual Location Location1 { get; set; }
    }
}
