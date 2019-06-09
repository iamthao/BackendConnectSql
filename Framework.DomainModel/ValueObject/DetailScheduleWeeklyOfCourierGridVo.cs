using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class DetailScheduleWeeklyOfCourierGridVo : ReadOnlyGridVo
    {
        public string Name { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public bool IsSchedule { get; set; }
        public int? HistoryScheduleId { get; set; }
        public string FrequencyEncode { get; set; }
        public string FrequencyEncodeNotSchedule {
            get
            {
                return "59 59 23 " + (CreatedOn != null ? ((DateTime)CreatedOn).AddMinutes(TimeZone).Day : 0) + " * ?";
                
            }
        }
        public string Frequency
        {
            get
            {
                if (!IsSchedule)
                {
                    FrequencyEncode = FrequencyEncodeNotSchedule;
                }
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
                if (!IsSchedule)
                {
                    FrequencyEncode = FrequencyEncodeNotSchedule;
                }
                if (!string.IsNullOrEmpty(FrequencyEncode))
                {

                    var fre = FrequencyEncode.Split(' ');
                    if (fre.Length > 0)
                    {
                        if (fre.Length == 6 && fre[3] != "*" && fre[4] == "*" && fre[5] == "?")
                        {

                            int[] intArray =Array.ConvertAll(fre[3].Split(','),int.Parse);
                            Array.Sort(intArray);
                            return string.Join(", ",intArray);
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
                if (!IsSchedule)
                {
                    FrequencyEncode = FrequencyEncodeNotSchedule;
                }
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
                            string [] sortedArray = initialArray.OrderBy(s => Enum.Parse(typeof(DayOfWeek), s)).ToArray() ;
                            return string.Join(", ", sortedArray);
                        }
                    }
                }

                return null;

            }
        }
        public List<DayOfWeek> ListDayOfWeeks
        {
            get
            {
                if (!IsSchedule)
                {
                    FrequencyEncode = FrequencyEncodeNotSchedule;
                }
                if (!string.IsNullOrEmpty(FrequencyEncode))
                {
                    var fre = FrequencyEncode.Split(' ');
                    if (fre.Length > 0)
                    {
                        if (fre.Length == 6 && fre[3] == "?")
                        {
                            var initialArray = new List<DayOfWeek>();
                            var i = 0;
                            foreach (var day in fre[5].Split(','))
                            {
                                switch (day)
                                {
                                    case "MON":
                                        initialArray.Add(DayOfWeek.Monday);
                                        break;
                                    case "TUE":
                                        initialArray.Add(DayOfWeek.Tuesday);
                                        break;
                                    case "WED":
                                        initialArray.Add(DayOfWeek.Wednesday);
                                        break;
                                    case "THU":
                                        initialArray.Add(DayOfWeek.Thursday);
                                        break;
                                    case "FRI":
                                        initialArray.Add(DayOfWeek.Friday);
                                        break;
                                    case "SAT":
                                        initialArray.Add(DayOfWeek.Saturday);
                                        break;
                                    case "SUN":
                                        initialArray.Add(DayOfWeek.Sunday);
                                        break;
                                }
                                i++;
                            }
                            return initialArray;
                        }
                    }
                }

                return null;

            }
        }
        public int TimeZone { get; set; }

        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime DurationStart { get; set; }
        public DateTime? DurationEnd { get; set; }
        public DateTime? CreatedOn { get; set; }
        public bool Monday
        {
            get { return GetDayOfWeek(DayOfWeek.Monday); }
        }
        public bool Tuesday
        {
            get { return GetDayOfWeek(DayOfWeek.Tuesday); }
        }
        public bool Wednesday
        {
            get { return GetDayOfWeek(DayOfWeek.Wednesday); }
        }
        public bool Thursday
        {
            get { return GetDayOfWeek(DayOfWeek.Thursday); }
        }
        public bool Friday
        {
            get { return GetDayOfWeek(DayOfWeek.Friday); }
        }
        public bool Saturday
        {
            get { return GetDayOfWeek(DayOfWeek.Saturday); }
        }
        public bool Sunday
        {
            get { return GetDayOfWeek(DayOfWeek.Sunday); }
        }

        public bool GetDayOfWeek(DayOfWeek dayOfWeek)
        {
            if (!IsSchedule)
            {
                FrequencyEncode = FrequencyEncodeNotSchedule;
            }
            if (!string.IsNullOrEmpty(FrequencyEncode))
            {
               var durationStart = DurationStart.AddMinutes(TimeZone);
                if (DurationEnd != null)
                {
                    var durationEnd = ((DateTime)DurationEnd).AddMinutes(TimeZone);
                    if (durationStart > (DateTime)FromDate && durationStart > (DateTime)ToDate)
                    {
                        return false;
                    }

                    if (durationStart >= (DateTime)FromDate && durationEnd >= ToDate)
                    {
                        FromDate = durationStart;
                    }
                    if (durationStart >= (DateTime)FromDate && durationEnd <= ToDate)
                    {
                        FromDate = durationStart;
                        ToDate = durationEnd;
                    }
                    if (durationStart <= (DateTime)FromDate && durationEnd >= FromDate && durationEnd <= ToDate)
                    {
                        ToDate = durationEnd;
                    }
                    if (durationEnd < FromDate && durationEnd < ToDate)
                    {
                        return false;
                    }
                }
                else
                {

                    if (durationStart > (DateTime)FromDate && durationStart > (DateTime)ToDate)
                    {
                        return false;
                    }

                    if (durationStart >= (DateTime)FromDate && durationStart <= ToDate)
                    {
                        FromDate = durationStart;
                    }
                }
                var fre = FrequencyEncode.Split(' ');
                if (fre.Length > 0)
                {

                    //daily
                    if (fre.Length == 6 && fre[3] == "*" && fre[4] == "*" && fre[5] == "?")
                    {
                        for (var day = ((DateTime)FromDate).Date; day.Date <= ((DateTime)ToDate).Date; day = day.AddDays(1))
                        {
                            if (day.DayOfWeek==dayOfWeek)
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                    //monthly
                    if (fre.Length == 6 && fre[3] != "*" && fre[4] == "*" && fre[5] == "?")
                    {
                        if (FromDate != null && ToDate != null)
                        {

                            var arrDayOfMonth = fre[3].Split(',');
                            if (arrDayOfMonth.Length > 0)
                            {
                                int[] myInts = Array.ConvertAll(arrDayOfMonth, int.Parse);
                                Array.Sort(myInts);

                                for (var day = ((DateTime)FromDate).Date; day.Date <= ((DateTime)ToDate).Date; day = day.AddDays(1))
                                {
                                    if (myInts.Contains(day.Day) && day.DayOfWeek == dayOfWeek)
                                    {
                                        return true;
                                    }
                                }

                            }
                        }
                    }
                    //weekly
                    if (fre.Length == 6 && fre[3] == "?")
                    {

                        for (var day = ((DateTime)FromDate).Date; day.Date <= ((DateTime)ToDate).Date; day = day.AddDays(1))
                        {
                            if (ListDayOfWeeks.Contains(day.DayOfWeek) && day.DayOfWeek == dayOfWeek)
                            {
                                return true;
                            }
                        }
                        return false;
                    }
                }
            }
            return false;
        }

    }
}
