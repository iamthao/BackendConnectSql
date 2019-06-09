using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Framework.DataAnnotations;

namespace Framework.DomainModel.Entities
{
    public class Schedule : Entity
    {
        [LocalizeRequired(FieldName = "Route name")]
        public string Name { get; set; }
        public int LocationFrom { get; set; }
        public int LocationTo { get; set; }
        public string Frequency { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime DurationStart { get; set; }
        public DateTime? DurationEnd { get; set; }
        public bool? IsNoDurationEnd { get; set; }
        public string Description { get; set; }
        public int CourierId { get; set; }
        public double? TimeZone { get; set; }
        public bool? IsWarning { get; set; }
        public virtual Location LocationFromObj { get; set; }
        public virtual Location LocationToObj { get; set; }
        public virtual Courier Courier { get; set; }
        [NotMapped]
        public bool? Confirm { get; set; }
        [NotMapped]
        public dynamic WarningInfo { get; set; }
        [NotMapped]
        public DateTime? CopyCreatedOn { get; set; }
    }
}
