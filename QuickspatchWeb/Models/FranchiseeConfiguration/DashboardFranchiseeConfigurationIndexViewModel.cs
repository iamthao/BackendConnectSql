namespace QuickspatchWeb.Models.FranchiseeConfiguration
{
    public class DashboardFranchiseeConfigurationIndexViewModel : DashboardGridViewModelBase<Framework.DomainModel.Entities.FranchiseeConfiguration>
    {
        public override string PageTitle
        {
            get
            {
                return "Franchisee Configuration";
            }
        }
    }
}