using System;
using System.Globalization;
using System.Web;

namespace Framework.Utility
{
    public static class DateTimeHelper
    {
        public static DateTime GetMinDate()
        {
            return new DateTime(1900, 1, 1);
        }

        /// <summary>
        /// Convert the passed datetime into client timezone.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToClientTime(this DateTime dt, string format)
        {
            var timeOffSet = HttpContext.Current.Session["timezoneoffset"];  // read the value from session

            if (timeOffSet != null)
            {
                var offset = int.Parse(timeOffSet.ToString());
                dt = dt.AddMinutes(-1 * offset);

                return dt.ToString(format);
            }

            // if there is no offset in session return the datetime in server timezone
            return dt.ToLocalTime().ToString(format);
        }

        public static double GetClientTimeZone()
        {
            var timeOffSet = HttpContext.Current.Session["timezoneoffset"];  // read the value from session

            if (timeOffSet != null)
            {
                var offset = int.Parse(timeOffSet.ToString());
                return (-1 * offset);
            }

            // if there is no offset in session return the datetime in server timezone
            return TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow).TotalMinutes;
        }

        public static DateTime ToUtcTimeFromClientTime(this DateTime dt)
        {
            if (dt <= DateTime.MinValue)
            {
                return DateTime.MinValue;
            }

            var timeOffSet = HttpContext.Current.Session["timezoneoffset"];  // read the value from session

            if (timeOffSet != null)
            {
                var offset = int.Parse(timeOffSet.ToString());
                dt = dt.AddMinutes(offset);

                return dt;
            }

            // if there is no offset in session return the datetime in server timezone
            return dt.ToLocalTime();
        }

        public static DateTime SetDateToCurrentDate(this DateTime dt)
        {
            var currentDate = DateTime.UtcNow;
            return new DateTime(currentDate.Year, currentDate.Month, currentDate.Day, dt.Hour, dt.Minute, 0, 0);
        }

        public static DateTime ToClientTimeDateTime(this DateTime dt)
        {
            var timeOffSet = HttpContext.Current.Session["timezoneoffset"];  // read the value from session

            if (timeOffSet != null)
            {
                var offset = int.Parse(timeOffSet.ToString());
                dt = dt.AddMinutes(-1 * offset);

                return dt;
            }

            // if there is no offset in session return the datetime in server timezone
            return dt.ToLocalTime();
        }


        public static DateTime GetStartDateTime(DateTime dt)
        {
            var offset = GetClientTimeZone();
            var currentDateParse = dt;
            var clientDate = currentDateParse.AddMinutes(offset);
            return currentDateParse.AddMilliseconds(-1 *
                                                 clientDate.Subtract(new DateTime(clientDate.Year, clientDate.Month, clientDate.Day)).TotalMilliseconds);
        }
        public static DateTime GetEndDateTime(DateTime dt)
        {          
            var startNowTime = GetStartDateTime(dt);
            return startNowTime.AddMinutes(1439);
        }

        public static int TotalDaysUsed(DateTime dt1, DateTime dt2)
        {
            var start = GetStartDateTime(dt1);
            var change = GetEndDateTime(dt2);
            var ts = change.Subtract(start);
            return (int)Math.Ceiling(ts.TotalDays);
        }

        public static int TotalMonthsUsed(DateTime inDate1, DateTime inDate2)
        {
            inDate1 = GetStartDateTime(inDate1);
            inDate2 = GetEndDateTime(inDate2);
            int m = ((inDate2.Year - inDate1.Year) * 12) + inDate2.Month - inDate1.Month;
            var newD1 = inDate1.AddMonths(m);
            if (CompareDateTime(newD1, inDate2) > 0)
            {
                newD1 = newD1.AddMonths(-1);
                m--;
            }

            var days = TotalDaysUsed(newD1, inDate2);//inDate2.Day - newD1.Day;
            if (days >= 1)
                return m + 1;

            return m;
        }

        /// <summary>
        /// Conpare datetime
        /// </summary>
        /// <param name="d1">destination</param>
        /// <param name="d2">source</param>
        /// <param name="format">foramt datetime</param>
        /// <returns>-1 is d2 > d1, 0 is d1 = d2, 1 is d1 > d2. </returns>
        public static int CompareDateTime(this DateTime d1, DateTime d2, string format = "MM/dd/yyyy HH:mm")
        {
            if (string.IsNullOrEmpty(format))
            {
                throw new Exception("Format invalid.");
            }

            var dr1 = DateTime.ParseExact(d1.ToString(format), format, CultureInfo.InvariantCulture);
            var dr2 = DateTime.ParseExact(d2.ToString(format), format, CultureInfo.InvariantCulture);
            return dr1 > dr2 ? 1 : dr2 > dr1 ? -1 : 0;
        }
    }
}