namespace QuickspatchWeb.Models.SystemConfiguration
{
    public class DashboardSystemConfigurationIndexViewModel : DashboardGridViewModelBase<Framework.DomainModel.Entities.SystemConfiguration>
    {
        public override string PageTitle
        {
            get
            {
                return "System Configuration";
            }
        }
    }
}