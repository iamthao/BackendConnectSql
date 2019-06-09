using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickspatchWeb.Models.CountryOrRegion
{
    public class DashboardCountryOrRegionDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.CountryOrRegion>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardCountryOrRegionShareViewModel>(parameters);
        }
    }
}