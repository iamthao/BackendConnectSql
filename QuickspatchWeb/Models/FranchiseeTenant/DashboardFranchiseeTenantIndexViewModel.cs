namespace QuickspatchWeb.Models.FranchiseeTenant
{
    public class DashboardFranchiseeTenantIndexViewModel : DashboardGridViewModelBase<Framework.DomainModel.Entities.FranchiseeTenant>
    {
        public override string PageTitle
        {
            get
            {
                return "Franchisee";
            }
        }
    }
}