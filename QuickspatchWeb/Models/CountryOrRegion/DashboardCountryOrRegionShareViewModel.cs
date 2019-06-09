using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickspatchWeb.Models.CountryOrRegion
{
    public class DashboardCountryOrRegionShareViewModel : DashboardSharedViewModel
    {
        public string Name { get; set; }
        public string CodeName { get; set; }
    }
}