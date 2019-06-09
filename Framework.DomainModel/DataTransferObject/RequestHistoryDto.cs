using System;
using System.Collections.Generic;

namespace Framework.DomainModel.DataTransferObject
{
    public class RequestHistoryDto : DtoBase
    {
        public string RequestNumber { get; set; }
        public LocationDto Locationfrom { get; set; }
        public LocationDto Locationto { get; set; }
        public int RequestType { get; set; }
        public List<NoteDto> NoteRequests { get; set; }
        public bool IsStat { get; set; }
        public string Description { get; set; }
        public bool IsScheduleCreated { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int RequestId { get; set; }
        public int ActionType { get; set; }
        public int LastRequestStatus { get; set; }
        public string Comment { get; set; }
        public DateTime TimeChanged { get; set; }
        public string Signature { get; set; }
        public DateTime? ReceivedTime { get; set; }
        public DateTime? AcceptedTime { get; set; }
        public DateTime? RejectedTime { get; set; }
        public DateTime? ActualStartTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public string CompletePicture { get; set; }
        public string CompleteNote { get; set; }
        public bool? IsAgreed { get; set; }
    }
}