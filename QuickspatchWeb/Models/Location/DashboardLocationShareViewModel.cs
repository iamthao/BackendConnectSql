using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickspatchWeb.Models.Location
{
    public class DashboardLocationShareViewModel : DashboardSharedViewModel
    {
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        public byte[] AvailableTime { get; set; }
        public DateTime? OpenHour { get; set; }
        public DateTime? CloseHour { get; set; }
        public double? Lat { get; set; }
        public double? Lng { get; set; }
        public bool AutoGetCityState { get; set; }
        //
        public string StateOrProvinceOrRegion { get; set; }//
        public int IdCountryOrRegion { get; set; }//
        //
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public string Type { get; set; }

    }
}