namespace QuickspatchWeb.Models.User
{
    public class DashboardUserDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.User>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardUserShareViewModel>(parameters);
        }
    }
}