using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Framework.DomainModel.Entities
{
    public class Courier : Entity
    {
        public Courier()
        {
            this.NoteRequests = new List<NoteRequest>();
            this.Requests = new List<Request>();
            this.Trackings = new List<Tracking>();
            this.Schedules = new List<Schedule>();
        }

        public int Status { get; set; }
        public string CarNo { get; set; }
        public string Imei { get; set; }
        public virtual User User { get; set; }
        public int? CurrentReq { get; set; }
        public double? CurrentLat { get; set; }
        public double? CurrentLng { get; set; }
        public double? CurrentVelocity { get; set; }
        public DateTime? ServiceResetTime { get; set; }
        public virtual ICollection<NoteRequest> NoteRequests { get; set; }
        public virtual ICollection<Request> Requests { get; set; }
        public virtual ICollection<Tracking> Trackings { get; set; }
        public virtual ICollection<Schedule> Schedules { get; set; }
    }
}
