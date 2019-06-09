namespace QuickspatchWeb.Models.HoldingRequest
{
    public class DashboardHoldingRequestDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.HoldingRequest>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardHoldingRequestShareViewModel>(parameters);
        }
    }
}