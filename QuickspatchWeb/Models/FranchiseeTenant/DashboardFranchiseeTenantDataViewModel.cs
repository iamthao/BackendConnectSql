namespace QuickspatchWeb.Models.FranchiseeTenant
{
    public class DashboardFranchiseeTenantDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.FranchiseeTenant>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardFranchiseeTenantShareViewModel>(parameters);
        }
    }
}