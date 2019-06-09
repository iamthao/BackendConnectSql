namespace QuickspatchWeb.Models.Schedule
{
    public class DashboardScheduleDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.Schedule>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardScheduleShareViewModel>(parameters);
        }
    }
}