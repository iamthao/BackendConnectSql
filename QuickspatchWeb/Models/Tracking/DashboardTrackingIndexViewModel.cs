namespace QuickspatchWeb.Models.Tracking
{
    public class DashboardTrackingIndexViewModel : DashboardGridViewModelBase<Framework.DomainModel.Entities.Tracking>
    {
        public override string PageTitle
        {
            get { return "Tracking"; }
        }
    }
}