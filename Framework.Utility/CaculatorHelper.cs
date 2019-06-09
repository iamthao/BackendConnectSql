using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using Microsoft.CSharp.RuntimeBinder;
using Org.BouncyCastle.Asn1.Cms;

namespace Framework.Utility
{
    public static class CaculatorHelper
    {
        public static int CaculateDuration(DateTime startDate, DateTime dueDate, DateTime? cancelDate,
            DateTime? completedDate)
        {
            if (cancelDate == null && completedDate == null)
            {
                if (DateTime.UtcNow < dueDate)
                {
                    return (int)(DateTime.UtcNow - startDate).TotalHours;
                }
                return (int)(DateTime.UtcNow - dueDate).TotalHours;
            }

            if (cancelDate != null)
            {
                return (int)(((DateTime)cancelDate) - startDate).TotalHours;
            }
            return (int)(((DateTime)completedDate) - startDate).TotalHours;
        }

        public static string CaculateFormatDuration(int duration)
        {
            if (duration < 0)
            {
                return "0d0h";
            }
            return (duration / 24).ToString(CultureInfo.InvariantCulture) + "d" +
                   (duration % 24).ToString(CultureInfo.InvariantCulture) + "h";
        }
       
        public static string ConvertIntToTime(int minutes)
        {
            //int mins = 0;
            //int h = 0;
            //int m = 0;
            //string s = "";
            //if (minutes <= 720)
            //{
            //    mins = minutes;
            //    h = mins / 60;
            //    m = mins % 60;
            //    s = "" + h.ToString("00") + ":" + m.ToString("00") + "  AM";
            //}
            //else if (minutes > 720)
            //{
            //    mins = minutes - 720;
            //    h = mins / 60;
            //    m = mins % 60;
            //    s = "" + h.ToString("00") + ":" + m.ToString("00") + "  PM";
            //}

            //return s;
            TimeSpan timeSpan = TimeSpan.FromMinutes(minutes);
            DateTime time = DateTime.Today.Add(timeSpan);
            return time.ToString("hh:mm tt");
        }


        public static string GetFullName(string firstName, string middleName, string lastName)
        {
            if (firstName == null || lastName == null)
            {
                return string.Empty;
            }

            return firstName + " " + lastName + (string.IsNullOrEmpty(middleName) ? "" : " " + middleName);
        }

        public static string GetFullAddress(string address1, string address2, string city, string state, string zip)
        {
            if (string.IsNullOrEmpty(address1) && string.IsNullOrEmpty(address2) && string.IsNullOrEmpty(city) && string.IsNullOrEmpty(city) && string.IsNullOrEmpty(zip))
            {
                return string.Empty;
            }
            var result = "";
            if (!string.IsNullOrEmpty(address1))
                result += address1;
            if (!string.IsNullOrEmpty(address2))
                result += ", " + address2;
            if (!string.IsNullOrEmpty(city))
                result += ", " + city;
            if (!string.IsNullOrEmpty(state))
                result += ", " + state;
            if (!string.IsNullOrEmpty(zip))
                result += " " + zip;

            return result;
        }

        public static string GetFullAddressCountry(string address1, string address2, string city, string stateorprovinceorregion, string zip, string countryorregion)
        {
            if (string.IsNullOrEmpty(address1) && string.IsNullOrEmpty(address2)
                && string.IsNullOrEmpty(city) && string.IsNullOrEmpty(stateorprovinceorregion)
                && string.IsNullOrEmpty(zip) && string.IsNullOrEmpty(countryorregion))
            {
                return string.Empty;
            }
            var result = "";
            if (!string.IsNullOrEmpty(address1))
                result += address1;
            if (!string.IsNullOrEmpty(address2))
                result += ", " + address2;
            if (!string.IsNullOrEmpty(city))
                result += ", " + city;
            if (!string.IsNullOrEmpty(stateorprovinceorregion))
                result += ", " + stateorprovinceorregion;
            if (!string.IsNullOrEmpty(zip))
                result += ", " + zip;
            if (!string.IsNullOrEmpty(countryorregion))
                result += ", " + countryorregion;
            return result;
        }

        public static byte[] GetBytes(string str)
        {
            byte[] bytes = new byte[str.Length * sizeof(char)];
            System.Buffer.BlockCopy(str.ToCharArray(), 0, bytes, 0, bytes.Length);
            return bytes;
        }

        public static string GetString(byte[] bytes)
        {
            char[] chars = new char[bytes.Length / sizeof(char)];
            System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
            return new string(chars);
        }

        public static long MeasureStopWatch(int iterations, Action action)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (var i = 0; i < iterations; i++)
            {
                action();
            }

            stopwatch.Stop();
            return stopwatch.ElapsedTicks;
        }

        public static bool IsPropertyExist(dynamic settings, string name)
        {
            try
            {
                var value = settings[name].Value;
                return true;
            }
            catch (RuntimeBinderException)
            {

                return false;
            }
        }
        public static string GetNamePackage(int packageId)
        {
            switch (packageId)
            {
                //1,2. (1-5) ; 3,4 : (6-25) ; 5,6 : (26-50) ; 7,8 (115)
                case 1:
                    return "QuickSpatch Package 10 Couriers - Paid annually";
                case 2:
                    return "QuickSpatch Package 10 Couriers - Paid monthly";
                case 3:
                    return "QuickSpatch Package 25 Couriers - Paid annually";
                case 4:
                    return "QuickSpatch Package 25 Couriers - Paid monthly";
                case 5:
                    return "QuickSpatch Package 50 Couriers - Paid annually";
                case 6:
                    return "QuickSpatch Package 50 Couriers - Paid monthly";
                //case 7:
                //    return "QuickSpatch Package 115 Couriers - Paid annually";
                //case 8:
                //    return "QuickSpatch Package 115 Couriers - Paid monthly";
            }
            return string.Empty;
        }

        public static decimal GetPricePackage(int packageId)
        {
            switch (packageId)
            {
                case 1: // 1-5   annually
                    return 5;//1440;
                case 2:  //1-5  monthly
                    return 5;//144;
                case 3:   //6-25 annually
                    return 5;//3000;
                case 4:    //6-25  monthly
                    return 5;//300;
                case 5:   // 26-50  annually
                    return 5;//4800;
                case 6:  //26-50   Monthly
                    return 5;//480;
                //case 7:   //51+  annually
                //    return 5988;
                //case 8:    //51+   Monthly
                //    return 624;
            }
            return 0;
        }

        public static int GetNumberUserAllow(int packageId)
        {
            switch (packageId)
            {
                case 1: 
                    return 5;
                case 2:  
                    return 5;
                case 3:  
                    return 25;
                case 4:
                    return 25;
                case 5:
                    return 50;
                case 6:
                    return 50;
                //case 7:
                //    return 115;
                //case 8:
                //    return 115;
            }
            return 2;
        }

        public static string GetDescriptionPackage(int packageId)
        {
            switch (packageId)
            {
                case 1:
                    return "Package (10 Couriers) - Paid Annually";
                case 2:
                    return "Package (10 Couriers) - Paid Monthly";
                case 3:
                    return "Package (25 Couriers) - Paid Annually";
                case 4:
                    return "Package (25 Couriers) - Paid Monthly";
                case 5:
                    return "Package (50 Couriers) - Paid Annually";
                case 6:
                    return "Package (50 Couriers) - Paid Monthly";
                //case 7:
                //    return "Package (115 Couriers) - Paid Annually";
                //case 8:
                //    return "Package (115 Couriers) - Paid Monthly";
            }
            return "Trial (2 Couriers)";
        }

        public static DateTime GetNextMonth(DateTime start)
        {
            var date = start;
            if (start.Year < DateTime.UtcNow.Year)
            {
                date = new DateTime(DateTime.UtcNow.Year, 1, start.Day);
            }

            var numberMonth = DateTime.UtcNow.Month - date.Month;
            if (numberMonth > 0)
            {
                if (date.Day > DateTime.UtcNow.Day)
                {
                    return date.AddMonths(numberMonth);
                }
                return date.AddMonths(numberMonth + 1);
            }

            if (date.Day > DateTime.UtcNow.Day)
            {
                return date;
            }

            return date.AddMonths(1);
        }

        public static string ConvertSecondsToHours(int seconds)
        {
            int numberMinutes = (int)(seconds / 60);

            var hour = (int)(numberMinutes / 60) > 0 ? (int)(numberMinutes / 60) : 0;
            var minutes = (int)(numberMinutes - (hour * 60));

            var textHour = hour > 0 ? (hour + " H ") : "";
            var textMinutes = minutes > 0 ? (minutes + " m") : "";
            if (hour > 0 || minutes > 0)
                return textHour + textMinutes;
            return "0";
        }

        public static string ConvertTextToHtml(this string value)
        {
            return value.Replace("\\r\\n", "<br />").Replace("\\n", "<br />");
        }

        public static int RemainMonth(DateTime start, DateTime end)
        {
            var a = (end - start).TotalDays;
            var b = a / 30;
            var c = (int)Math.Ceiling(b);
            return c;       
        }

        //lay so thang or so ngay giua 2 khoan thoi gian
        public static int DateDiffAsString(DateTime inDate1, DateTime inDate2, int type)
        {
            //type= 1 :lay so month; type=2:lay so day
            int y = 0;
            int m = 0;
            int d = 0;

            //make sure date1 is before (or equal to) date2..
            DateTime date1 = inDate1 <= inDate2 ? inDate1 : inDate2;
            DateTime date2 = inDate1 <= inDate2 ? inDate2 : inDate1;
            DateTime temp1;

            if (DateTime.IsLeapYear(date1.Year) &&
              !DateTime.IsLeapYear(date2.Year) &&
              date1.Month == 2 &&
              date1.Day == 29)
            {
                temp1 = new DateTime(date2.Year, date1.Month, date1.Day - 1);
            }
            else
            {
                temp1 = new DateTime(date2.Year, date1.Month, date1.Day);
            }


            y = date2.Year - date1.Year - (temp1 > date2 ? 1 : 0);
            m = date2.Month - date1.Month + (12 * (temp1 > date2 ? 1 : 0));
            d = date2.Day - date1.Day;
            if (d < 0)
            {
                if (date2.Day == DateTime.DaysInMonth(date2.Year, date2.Month) &&
                    (date1.Day >= DateTime.DaysInMonth(date2.Year, date2.Month) ||
                     date1.Day >= DateTime.DaysInMonth(date2.Year, date1.Month)))
                {
                    d = 0;
                }
                else
                {
                    m--;
                    if (DateTime.DaysInMonth(date2.Year, date2.Month) == DateTime.DaysInMonth(date1.Year, date1.Month) && date2.Month != date1.Month)
                    {
                        int dayBase = date2.Month - 1 > 0 ? DateTime.DaysInMonth(date2.Year, date2.Month - 1) : 31;
                        d = dayBase + d;
                    }
                    else
                    {
                        d = DateTime.DaysInMonth(date2.Year, date2.Month == 1 ? 12 : date2.Month - 1) + d;
                    }
                }
            }

            //string ts = "";

            ////if (y > 0) 
            //    ts += y + "/";
            ////if (m > 0)
            //    ts += m + "/";
            ////if (d > 0)
            //    ts += d;

            if (type == 1)
                return m;
            return d;
        }

     
    }


}
