using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickspatchWeb.Models.Location
{
    public class DashboardLocationDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.Location>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardLocationShareViewModel>(parameters);
        }
    }
}