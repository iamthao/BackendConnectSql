namespace QuickspatchWeb.Models.Courier
{
    public class DashboardCourierDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.Courier>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardCourierShareViewModel>(parameters);
        }
    }
}