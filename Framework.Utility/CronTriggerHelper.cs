using System;

namespace Framework.Utility
{
    public static class CronTriggerHelper
    {
        public static string FrequencySelected(string cronString)
        {
            if (string.IsNullOrEmpty(cronString)) return "daily";

            var fre = cronString.Split(' ');
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

            return "daily";
        }

        public static string FrequencyWeek(string cronString)
        {
            string result = "";
            if (string.IsNullOrEmpty(FrequencySelected(cronString))) return "";

            var fre = cronString.Split(' ');
            if (FrequencySelected(cronString) == "weekly" && fre.Length > 5)
            {
                result = fre[5];
            }

            return result;
        }

        public static string FrequencyMonth(string cronString)
        {
            string result = "";
            if (string.IsNullOrEmpty(FrequencySelected(cronString))) return "";

            var fre = cronString.Split(' ');

            if (FrequencySelected(cronString) == "monthly" && fre.Length > 3)
            {
                result = fre[3];
            }

            return result;
        }
    }
}