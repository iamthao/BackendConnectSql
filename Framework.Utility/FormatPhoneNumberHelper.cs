using System;
using System.Text.RegularExpressions;

namespace Framework.Utility
{
    public static class FormatPhoneNumberHelper
    {
        public static string RemoveFormatPhone(this string maskPhoneNumber)
        {
            if (!string.IsNullOrEmpty(maskPhoneNumber))
            {
                return maskPhoneNumber.Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "");
            }
            return maskPhoneNumber;
        }
        public static string ApplyFormatPhone(this string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber)) return "";

            phoneNumber = RemoveFormat(phoneNumber);

            if (string.IsNullOrEmpty(phoneNumber) || phoneNumber.Length != 10) return phoneNumber;

            return String.Format("{0:(000) 000-0000}", double.Parse(phoneNumber));
        }

        public static string ApplyFormatSSN(this string number)
        {
            if (string.IsNullOrEmpty(number)) return "";

            number = RemoveFormat(number);

            if (string.IsNullOrEmpty(number) || number.Length != 9) return number;

            return String.Format("{0:000-00-0000}", double.Parse(number));
        }

        public static string RemoveFormat(this string number)
        {
            return string.IsNullOrEmpty(number) ? number : Regex.Replace(number, "[^.0-9]+", "");
        }


        public static string RemoveNpiFormat(this string number)
        {
            return number.Replace("_", "");
        }

        public static bool IsFormatPhone(this string phoneNumber)
        {
            var match = Regex.Match(phoneNumber, @"((\(\d{3}\) ?)|(\d{3}-))?\d{3}-\d{4}");
            return match.Success;
        }

        public static int ParseToInt(this string number)
        {
            int result = 0;
            int.TryParse(number, out result);
            return result;
        }
    }
}
