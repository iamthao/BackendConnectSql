using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class ScheduleOfCourierGridVo : ReadOnlyGridVo
    {
        public string Name { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string FrequencyEncode { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool IsWarning { get; set; }

        public string StrStartTime { get { return StartTime.ToClientTime("HH:mm"); } }
        public string StrEndTime { get { return EndTime.ToClientTime("HH:mm"); } }
        public string Frequency
        {
            get
            {
                if (!string.IsNullOrEmpty(FrequencyEncode))
                {
                    var fre = FrequencyEncode.Split(' ');
                    if (fre.Length > 0)
                    {
                        if (fre.Length == 6 && fre[3] == "*" && fre[4] == "*" && fre[5] == "?")
                        {
                            return "daily";
                        }
                        if (fre.Length == 6 && fre[3] != "*" && fre[4] == "*" && fre[5] == "?")
                        {
                            return "monthly";
                        }
                        return "weekly";
                    }
                }

                return "";

            }
        }
        public string ByDates
        {
            get
            {
                if (!string.IsNullOrEmpty(FrequencyEncode))
                {
                    var fre = FrequencyEncode.Split(' ');
                    if (fre.Length > 0)
                    {
                        if (fre.Length == 6 && fre[3] != "*" && fre[4] == "*" && fre[5] == "?")
                        {

                            int[] intArray = Array.ConvertAll(fre[3].Split(','), int.Parse);
                            Array.Sort(intArray);
                            return string.Join(", ", intArray);
                        }
                    }
                }

                return null;

            }
        }
        public string ByDays
        {
            get
            {
                if (!string.IsNullOrEmpty(FrequencyEncode))
                {
                    var fre = FrequencyEncode.Split(' ');
                    if (fre.Length > 0)
                    {
                        if (fre.Length == 6 && fre[3] == "?")
                        {
                            string[] initialArray = new string[fre[5].Split(',').Length];
                            var i = 0;
                            foreach (var day in fre[5].Split(','))
                            {
                                switch (day)
                                {
                                    case "MON":
                                        initialArray[i] = "Monday";
                                        break;
                                    case "TUE":
                                        initialArray[i] = "Tuesday";
                                        break;
                                    case "WED":
                                        initialArray[i] = "Wednesday";
                                        break;
                                    case "THU":
                                        initialArray[i] = "Thursday";
                                        break;
                                    case "FRI":
                                        initialArray[i] = "Friday";
                                        break;
                                    case "SAT":
                                        initialArray[i] = "Saturday";
                                        break;
                                    case "SUN":
                                        initialArray[i] = "Sunday";
                                        break;
                                }
                                i++;
                            }
                            string[] sortedArray = initialArray.OrderBy(s => Enum.Parse(typeof(DayOfWeek), s)).ToArray();
                            return string.Join(", ", sortedArray);
                        }
                    }
                }

                return null;

            }
        }

        public DateTime DurationStart { get; set; }
        public DateTime? DurationEnd { get; set; }
        public bool IsExpried
        {
            get
            {
                var offset = DateTimeHelper.GetClientTimeZone();
                var currentDateParse = DateTime.UtcNow;
                var clientDate = currentDateParse.AddMinutes(offset);
                var startNowTime = currentDateParse.AddMilliseconds(-1 *
                                                     clientDate.Subtract(new DateTime(clientDate.Year, clientDate.Month, clientDate.Day)).TotalMilliseconds);
                if (IsNoDurationEnd.GetValueOrDefault())
                {
                    return false;
                }
                if (!IsNoDurationEnd.GetValueOrDefault() && DateTimeHelper.CompareDateTime(DurationEnd.GetValueOrDefault(),startNowTime) >= 0)
                    return false;
                return true;
            }
        }

        public bool? IsNoDurationEnd { get; set; }

        public DateTime? DateStartNow
        {
            get
            {
                var offset = DateTimeHelper.GetClientTimeZone();
                var currentDateParse = DateTime.UtcNow;
                var clientDate = currentDateParse.AddMinutes(offset);
                return currentDateParse.AddMilliseconds(-1 *
                                                     clientDate.Subtract(new DateTime(clientDate.Year, clientDate.Month, clientDate.Day)).TotalMilliseconds);
            }
        }

        public DateTime? CopyCreatedOn { get; set; }
    }
}
