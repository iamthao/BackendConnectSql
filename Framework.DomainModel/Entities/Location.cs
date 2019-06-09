using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Framework.DomainModel.Entities
{
    public class Location : Entity
    {
        public Location()
        {
            this.HoldingRequestsFrom = new List<HoldingRequest>();
            this.HoldingRequestsTo = new List<HoldingRequest>();
            this.RequestsOfLocationTo = new List<Request>();
            this.RequestsOfLocationFrom = new List<Request>();
            this.SchedulesFrom = new List<Schedule>();
            this.SchedulesTo = new List<Schedule>();
        }

        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public byte[] AvailableTime { get; set; }
        public int? OpenHour { get; set; }
        public int? CloseHour { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public bool? AutoGetCityState { get; set; }

        public string StateOrProvinceOrRegion { get; set; }
        [ForeignKey("CountryOrRegion")] 
        public int? IdCountryOrRegion { get; set; }

        public virtual CountryOrRegion CountryOrRegion { get; set; }
        public virtual ICollection<HoldingRequest> HoldingRequestsFrom { get; set; }
        public virtual ICollection<HoldingRequest> HoldingRequestsTo { get; set; }

        public virtual ICollection<Schedule> SchedulesFrom { get; set; }
        public virtual ICollection<Schedule> SchedulesTo { get; set; }

        public virtual ICollection<Request> RequestsOfLocationFrom { get; set; }

        public virtual ICollection<Request> RequestsOfLocationTo { get; set; }
    }
}
