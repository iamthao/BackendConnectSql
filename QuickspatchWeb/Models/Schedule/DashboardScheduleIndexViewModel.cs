namespace QuickspatchWeb.Models.Schedule
{
    public class DashboardScheduleIndexViewModel : DashboardGridViewModelBase<Framework.DomainModel.Entities.Schedule>
    {
        public override string PageTitle
        {
            get
            {
                return "Schedule";
            }
        }
    }
}