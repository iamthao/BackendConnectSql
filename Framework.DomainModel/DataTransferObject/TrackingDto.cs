using System;
using System.Collections.Generic;

namespace Framework.DomainModel.DataTransferObject
{
    public class TrackingDto : DtoBase
    {
        public List<int> RequestIds { get; set; }
        public int CourierId { get; set; }
        public string Address { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public double? Distance { get; set; }
        public DateTime? TimeTracking { get; set; }
        public double? Direction { get; set; }
        public double? Speed { get; set; }
    }
}