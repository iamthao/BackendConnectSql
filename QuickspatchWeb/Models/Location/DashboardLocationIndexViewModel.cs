using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickspatchWeb.Models.Location
{
    public class DashboardLocationIndexViewModel : DashboardGridViewModelBase<Framework.DomainModel.Entities.Location>
    {
        public override string PageTitle
        {
            get
            {
                return "Location";
            }
        }

        public string WebsiteUrl { get; set; }
    }
}