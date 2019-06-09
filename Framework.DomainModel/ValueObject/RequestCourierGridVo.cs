using System;
using System.Globalization;
using Framework.DomainModel.Common;
using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class RequestCourierGridVo : ReadOnlyGridVo
    {
        public string RequestNo { get; set; }
        public int? CourierId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Courier
        {
            get { return CaculatorHelper.GetFullName(FirstName, MiddleName, LastName); }
        }
        public int LocationFromId { get; set; }
        public string LocationFromName { get; set; }
        public double? LocationFromLat { get; set; }
        public double? LocationFromLng { get; set; }

        public int LocationToId { get; set; }
        public string LocationToName { get; set; }
        public double? LocationToLat { get; set; }
        public double? LocationToLng { get; set; }

        public int StatusId { get; set; }
        public bool IsSchedule { get; set; }
        public string Status
        {
            get
            {
                return StatusId <= 0 ? "" : (StatusId.GetNameByValue<StatusRequest>() == "NotSent" ? "Not Sent" : StatusId.GetNameByValue<StatusRequest>());
            }
        }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? SendingTime { get; set; }
        public bool IsWarning { get; set; }
    }


}