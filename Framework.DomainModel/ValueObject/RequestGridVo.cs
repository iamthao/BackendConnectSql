using System;
using System.Globalization;
using Framework.DomainModel.Common;
using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class RequestGridVo : ReadOnlyGridVo
    {
        public string RequestNo { get; set; }
        public int? CourierId { get; set; }
        public string FirstNameCourier { get; set; }
        public string MiddleNameCourier { get; set; }
        public string LastNameCourier { get; set; }
        public string CourierSearch { get; set; }
        public string Courier
        {
            get { return Framework.Utility.CaculatorHelper.GetFullName(FirstNameCourier, MiddleNameCourier, LastNameCourier); }
        }
        public int LocationFromId { get; set; }
        public int LocationToId { get; set; }
        public string LocationFromName { get; set; }
        public string LocationToName { get; set; }
        public string Type { get; set; }
        public int StatusId { get; set; }
        public bool IsExpired { get; set; }
        public bool IsSchedule { get; set; }
        public string Status
        {
            get
            {
                return StatusId <= 0 ? "" : (StatusId.GetNameByValue<StatusRequest>() == "NotSent" ? "Not Sent" : StatusId.GetNameByValue<StatusRequest>());
            }
        }
        public DateTime? StartTime { get; set; }
        public DateTime? StartTimeNoFormat { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? EndTimeNoFormat { get; set; }
        public string StrStartTime
        {
            get
            {
                if (StartTime != null)
                    return ((DateTime)StartTime).ToString("MM/dd/yyyy hh:mm tt");
                return "";
            }
        }
        public string StrEndTime
        {
            get
            {
                if (EndTime != null)
                    return ((DateTime)EndTime).ToString("MM/dd/yyyy hh:mm tt");
                return "";
            }
        }
        public DateTime? SendingTime { get; set; }
        public DateTime? TimeNoFormat { get; set; }
        public string Time
        {
            get
            {
                if (TimeNoFormat != null)
                    return ((DateTime) TimeNoFormat).ToString("MM/dd/yyyy hh:mm tt");
                return "";
            }
        }

        public string Note { get; set; }
        public DateTime? CreatedDateNoFormat { get; set; }
        public string FirstNameCreatedBy { get; set; }
        public string MiddleNameCreatedBy { get; set; }
        public string LastNameCreatedBy { get; set; }
        public string CreatedBy
        {
            get { return Framework.Utility.CaculatorHelper.GetFullName(FirstNameCreatedBy, MiddleNameCreatedBy, LastNameCreatedBy); }
        } 

        public bool IsActiveRequest { get; set; }
        public bool IsAgreed { get; set; }
        public bool IsWarning { get; set; }
        public DateTime? CreatedOn { get; set; }

    }


}