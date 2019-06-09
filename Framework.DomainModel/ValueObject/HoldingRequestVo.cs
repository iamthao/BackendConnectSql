using System;
using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class HoldingRequestVo : ReadOnlyGridVo
    {
        public int LocationFromId { get; set; }
        public int LocationToId { get; set; }
        public string LocationFrom { get; set; }
        public string LocationTo { get; set; }
        public string Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? StartTimeNoFormat { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? EndTimeNoFormat { get; set; }
        public DateTime? SendDate { get; set; }
        public bool OutOfDate {
            get
            {
                var now = DateTime.UtcNow.ToClientTimeDateTime();
                var sendDate = SendDate.GetValueOrDefault().ToClientTimeDateTime();
                if (new DateTime(sendDate.Year, sendDate.Month, sendDate.Day, 0, 0, 0) <
                        new DateTime(now.Year, now.Month, now.Day, 0, 0, 0))
                {
                    return true;
                }
                if (new DateTime(sendDate.Year, sendDate.Month, sendDate.Day, 0, 0, 0) >
                       new DateTime(now.Year, now.Month, now.Day, 0, 0, 0))
                {
                    return false;
                }              
                else {
                    var startTime = StartTime.GetValueOrDefault().ToClientTimeDateTime();
                    if (
                        new DateTime(now.Year, now.Month, now.Day, startTime.Hour,
                            startTime.Minute, 0) < now)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public bool IsShowSend
        {
            get
            {
                var now = DateTime.UtcNow.ToClientTimeDateTime();
                var sendDate = SendDate.GetValueOrDefault().ToClientTimeDateTime();
                if (new DateTime(sendDate.Year, sendDate.Month, sendDate.Day, 0, 0, 0) ==
                        new DateTime(now.Year, now.Month, now.Day, 0, 0, 0))
                {
                    return true;
                }
                return false;
            }
        }

        //public string SendTimeToClient
        //{
        //    get { return SendDate.GetValueOrDefault().ToClientTimeDateTime().ToString(); }
        //}

        //public string StartTimeToClient
        //{
        //    get { return StartTime.GetValueOrDefault().ToClientTimeDateTime().ToString(); }
        //}

        public string StartTimeToClientnow
        {
            get
            {
                var now = DateTime.UtcNow.ToClientTimeDateTime();
                return now.ToString();
            }
        }
        
    }
}