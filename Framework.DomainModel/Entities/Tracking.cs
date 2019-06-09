using System;
using System.Collections.Generic;

namespace Framework.DomainModel.Entities
{
    public class Tracking : Entity
    {
        public int? RequestId { get; set; }
        public int CourierId { get; set; }
        public string Address { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public double? Distance { get; set; }
        public double? Velocity { get; set; }
        public DateTime? TimeTracking { get; set; }
        public double? Direction { get; set; }
        public virtual Courier Courier { get; set; }
        public virtual Request Request { get; set; }
    }
}
