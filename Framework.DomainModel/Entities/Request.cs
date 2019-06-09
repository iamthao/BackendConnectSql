using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.DomainModel.Entities
{
    public class Request : Entity
    {
        public Request()
        {
            this.NoteRequests = new List<NoteRequest>();
            this.Trackings = new List<Tracking>();
            this.RequestHistorys = new List<RequestHistory>();
        }

        public string RequestNo { get; set; }
        public int LocationFrom { get; set; }
        public int LocationTo { get; set; }
        public int? CourierId { get; set; }
        public bool? IsStat { get; set; }
        public DateTime? SendingTime { get; set; }
        public int Status { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime? ReceivedTime { get; set; }
        public DateTime? AcceptedTime { get; set; }
        public DateTime? RejectedTime { get; set; }
        public DateTime? ActualStartTime { get; set; }
        public DateTime? ActualEndTime { get; set; }
        public int? HistoryScheduleId { get; set; }
        public int? PriorityNumber { get; set; }
        public double? ExpiredTime { get; set; }
        public bool? IsAgreed { get; set; }
        public bool? IsExpired { get; set; }
        public byte[] Signature { get; set; }
        public byte[] CompletePicture { get; set; }
        public string CompleteNote { get; set; }
        public double? EstimateDistance { get; set; }
        public int? EstimateTime { get; set; }

        public bool? IsWarning { get; set; }
        public int? DistanceEndFrom { get; set; }
        public int? TimeEndFrom { get; set; }

        [NotMapped]
        public bool? SaveSystemEvent { get; set; }
        public virtual AutomateSendRequest AutomateSendRequest { get; set; }
        public virtual Courier Courier { get; set; }
        public virtual Location LocationToObj { get; set; }
        public virtual Location LocationFromObj { get; set; }
        public virtual ICollection<NoteRequest> NoteRequests { get; set; }
        public virtual ICollection<Tracking> Trackings { get; set; }
        public virtual ICollection<RequestHistory> RequestHistorys { get; set; }
        [NotMapped]
        public bool? Confirm { get; set; }
        [NotMapped]
        public dynamic WarningInfo { get; set; }
    }
}
