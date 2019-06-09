namespace QuickspatchWeb.Models.Module
{
    public class DashboardModuleDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.Module>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardModuleShareViewModel>(parameters);
        }
    }
}