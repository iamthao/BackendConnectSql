using System;
using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class RequestReportVo : ReadOnlyGridVo
    {
        public string RequestNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public DateTime? RequestDate { get; set; }

        public string RequestDateFormat
        {
            get { return DateTimeHelper.GetEndDateTime(RequestDate.GetValueOrDefault()).ToShortDateString(); }
        }

        public string FullName
        {
            get { return Framework.Utility.CaculatorHelper.GetFullName(FirstName, MiddleName, LastName); }
        }
       
        public double? ActualDistance { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public string EstimateTime
        {
            get
            {
                var total = (EndTime - StartTime).TotalHours;
                return Math.Round(total, 2).ToString();
            }
        }

        public DateTime? ActualStartTime { get; set; }
        public DateTime? ActualEndTime { get; set; }

        public string RealTime
        {
            get
            {
                if (ActualEndTime != null && ActualEndTime != null)
                {
                    var total = (ActualEndTime.GetValueOrDefault() - ActualStartTime.GetValueOrDefault()).TotalHours;
                    return Math.Round(total, 2).ToString();
                }
                return "0";
            }
        }

        public double? EstimateDistance { get; set; }
        //public int? EstimateTime { get; set; }

        public string TextEstimateDistance
        {
            get
            {
                var text = EstimateDistance != null ? EstimateDistance.MetersToMiles(2).ToString() : "0";
                return text;
            }
        }

        public string TextActualDistance
        {
            get
            {
                var text = ActualDistance != null ? ActualDistance.MetersToMiles(2).ToString(): "0";
                return text;
            }
        }

    }
}