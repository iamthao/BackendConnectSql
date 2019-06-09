using System;
using System.Collections.Generic;

namespace Framework.DomainModel.DataTransferObject
{
    public class UpdateRequestDto: DtoBase
    {
        public int RequestId { get; set; }
        public int RequestType { get; set; }
        public List<NoteDto> NoteDto { get; set; }
        public int CourierId { get; set; }
        public int PriorityNumber { get; set; }
        public string Signature { get; set; }
        public string CompletePicture { get; set; }
        public string CompleteNote { get; set; }
        public DateTime? ReceivedTime { get; set; }
        public DateTime? AcceptedTime { get; set; }
        public DateTime? RejectedTime { get; set; }
        public DateTime? ActualStartTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public bool? IsAgreed { get; set; }
    }
}