using System;
using System.Collections.Generic;
using System.Linq;
using Framework.Utility;

namespace Framework.DomainModel.ValueObject
{
    public class DetailScheduleMonthlyOfCourierGridVo : ReadOnlyGridVo
    {
        public bool IsSchedule { get; set; }
        public string Name { get; set; }
        public string FrequencyEncode { get; set; }
        public int? HistoryScheduleId { get; set; }
        public string FrequencyEncodeNotSchedule
        {
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

                            int[] intArray = Array.ConvertAll(fre[3].Split(','), int.Parse);
                            Array.Sort(intArray);
                            return string.Join(", ", intArray);
                        }
                    }
                }

                return null;

            }
        }
        public List<DayOfWeek> ByDays
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

        public List<Route> ListRoute
        {
            get
            {
                if (!IsSchedule)
                {
                    FrequencyEncode = FrequencyEncodeNotSchedule;
                }
                var listRoute = new List<Route>();
                if (!string.IsNullOrEmpty(FrequencyEncode))
                {
                    var durationStart = DurationStart.AddMinutes(TimeZone);
                    if (DurationEnd != null)
                    {
                        var durationEnd = ((DateTime)DurationEnd).AddMinutes(TimeZone);
                        if (durationStart > (DateTime)FromDate && durationStart > (DateTime)ToDate)
                        {
                            return null;
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
                            return null;
                        }
                    }
                    else
                    {

                        if (durationStart > (DateTime)FromDate && durationStart > (DateTime)ToDate)
                        {
                            return null;
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
                                var route = new Route
                                {
                                    Id=Id,
                                    Name=Name,
                                    Date = day.Day,
                                    From = From,
                                    To = To,
                                    IsSchedule = true,
                                };
                                listRoute.Add(route);
                            }
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
                                        if (myInts.Contains(day.Day))
                                        {

                                            var route = new Route
                                            {
                                                Id = Id,
                                                Name = Name,
                                                Date = day.Day,
                                                From = From,
                                                To = To,
                                                HistoryScheduleId = HistoryScheduleId,
                                            };
                                            listRoute.Add(route);
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
                                if (ByDays.Contains(day.DayOfWeek))
                                {
                                    var route = new Route
                                    {
                                        Id = Id,
                                        Name = Name,
                                        Date = day.Day,
                                        From = From,
                                        To = To,
                                        HistoryScheduleId = HistoryScheduleId,
                                    };
                                    listRoute.Add(route);
                                }
                            }
                        }
                    }
                }
                return listRoute;

            }
        }

        public string From { get; set; }
        public string To { get; set; }
    }

    public class Route
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public bool IsSchedule { get; set; }
        public int? HistoryScheduleId { get; set; }
    }
}
