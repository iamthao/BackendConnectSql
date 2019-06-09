using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.ValueObject
{
    public class WarningInfoVo
    {
        public string PreviousRequestNo { get; set; }
        public string PreviousFromAddress { get; set; }
        public string PreviousToAddress { get; set; }
        public string PreviousFromAddressName { get; set; }
        public string PreviousToAddressName { get; set; }
        public DateTime? PreviousSendingTime { get; set; }
        public DateTime? PreviousStartTime { get; set; }
        public DateTime? PreviousEndTime { get; set; }

        public string RequestNo { get; set; }
        public string FromAddress { get; set; }
        public string ToAddressName { get; set; }
        public string FromAddressName { get; set; }
        public string ToAddress { get; set; }
        public double DistanceEndFrom { get; set; }
        public string StrDistanceEndFrom { get { return DistanceEndFrom == 0 ? "0" : MetersToMiles(DistanceEndFrom); } }
        public double DistanceFromTo { get; set; }
        public string StrDistanceFromTo { get { return DistanceFromTo == 0 ? "0" : MetersToMiles(DistanceFromTo); } }
        public double TimeEndFrom { get; set; }
        public string StrTimeEndFrom { get { return TimeEndFrom == 0 ? "0 mins" : ConvertSecondsToHours(TimeEndFrom); } }
        public double TimeFromTo { get; set; }
        public string StrTimeFromEnd { get { return TimeFromTo == 0 ? "0 mins" : ConvertSecondsToHours(TimeFromTo); } }

        public DateTime SendingTime { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public DateTime EstimateActualTime
        {
            get
            {
                if (PreviousEndTime != null)
                {
                    var dateTime = PreviousEndTime.GetValueOrDefault().ToString("MM/dd/yyyy HH:mm");
                    var timeEndFrom = (int) TimeEndFrom/60;
                    var timeFromTo = (int)TimeFromTo / 60;
                    return (DateTime.Parse(dateTime).AddMinutes(timeEndFrom).AddMinutes(timeFromTo));

                }
                return  SendingTime.AddSeconds(TimeFromTo);
            }
        }
        public bool IsUpdate { get; set; }
        private  string MetersToMiles(double meters, int digits = 2)
        {
            return meters == 0 ? "0 mi" : Math.Round(meters * 0.000621371, digits).ToString() + " mi";
        }
        private string ConvertSecondsToHours(double seconds)
        {
            int numberMinutes = (int)(seconds / 60);

            var hour = (int)(numberMinutes / 60) > 0 ? (int)(numberMinutes / 60) : 0;
            var minutes = (int)(numberMinutes - (hour * 60));

            var textHour = hour > 0 ? (hour + " H ") : "";
            var textMinutes = minutes > 0 ? (minutes + "") : "";
            if (hour > 0 || minutes > 0)
                return textHour + textMinutes;
            return "0";
        }
    }

    
}
