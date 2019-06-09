using System;
using System.Collections.Generic;
using System.Linq;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;

namespace QuickspatchWeb.Models.Schedule
{
    public class DashboardScheduleShareViewModel : DashboardSharedViewModel
    {
        public string Name { get; set; }
        public int LocationFrom { get; set; }
        public int LocationTo { get; set; }
        public string Frequency { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? DurationStart { get; set; }
        public DateTime? DurationEnd { get; set; }
        public bool? IsNoDurationEnd { get; set; }
        public string Description { get; set; }
        public int CourierId { get; set; }
        public int? ExpiredTime { get; set; }
        public LookupItemVo LocationFromDataSource { get; set; }
        public LookupItemVo LocationToDataSource { get; set; }
        public bool? Confirm { get; set; }

        public string FrequencySelected
        {
            get
            {
                if (string.IsNullOrEmpty(Frequency)) return "daily";

                var fre = Frequency.Split(' ');
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
        }

        public string FrequencyWeek
        {
            get
            {
                string result = "";
                if (string.IsNullOrEmpty(Frequency)) return "";

                var fre = Frequency.Split(' ');
                if (FrequencySelected == "weekly" && fre.Length > 5)
                {
                    result = fre[5];
                }

                return result;
            }
        }

        public string FrequencyMonth
        {
            get
            {
                string result = "";
                if (string.IsNullOrEmpty(Frequency)) return "";

                var fre = Frequency.Split(' ');
                
                if (FrequencySelected == "monthly" && fre.Length > 3)
                {
                    result = fre[3];
                }

                return result;
            }
        }
    }
}