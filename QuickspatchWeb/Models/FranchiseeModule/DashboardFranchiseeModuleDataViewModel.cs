namespace QuickspatchWeb.Models.FranchiseeModule
{
    public class DashboardFranchiseeModuleDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.FranchiseeModule>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardFranchiseeModuleShareViewModel>(parameters);
        }
    }
}