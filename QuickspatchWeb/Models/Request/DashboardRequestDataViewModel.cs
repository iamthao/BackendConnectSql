namespace QuickspatchWeb.Models.Request
{
    public class DashboardRequestDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.Request>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardRequestShareViewModel>(parameters);
        }
    }
}