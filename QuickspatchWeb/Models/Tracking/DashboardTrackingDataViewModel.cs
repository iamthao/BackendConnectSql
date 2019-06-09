namespace QuickspatchWeb.Models.Tracking
{
    public class DashboardTrackingDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.Tracking>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardTrackingShareViewModel>(parameters);
        }
    }
}