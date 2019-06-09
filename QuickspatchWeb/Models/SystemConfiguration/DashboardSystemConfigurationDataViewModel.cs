namespace QuickspatchWeb.Models.SystemConfiguration
{
    public class DashboardSystemConfigurationDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.SystemConfiguration>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardSystemConfigurationShareViewModel>(parameters);
        }

        
    }
}