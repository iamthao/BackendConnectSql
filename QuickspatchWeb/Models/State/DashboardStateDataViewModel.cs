
namespace QuickspatchWeb.Models.State
{
    public class DashboardStateDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.State>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardStateShareViewModel>(parameters);
        }
    }
}