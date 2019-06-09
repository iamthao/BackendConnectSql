using System;
using System.Collections.Generic;

namespace Framework.DomainModel.DataTransferObject
{
    public class RequestDto : DtoBase
    {
        public string RequestNumber { get; set; }
        public LocationDto Locationfrom { get; set; }
        public LocationDto Locationto { get; set; }
        public double DistanceFrom { get; set; }
        public int RequestType { get; set; }

        public bool IsStat { get; set; }
        public string Description { get; set; }
        public List<NoteDto> NoteRequests { get; set; }
        public bool IsScheduleCreated { get; set; }
        public DateTime? ReceivedTime { get; set; }
        public DateTime? AcceptedTime { get; set; }
        public DateTime? RejectedTime { get; set; }
        public DateTime? ActualStartTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }

        public int? PriorityNumber { get; set; }

    }
}