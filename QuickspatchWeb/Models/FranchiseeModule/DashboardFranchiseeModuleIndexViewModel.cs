namespace QuickspatchWeb.Models.FranchiseeModule
{
    public class DashboardFranchiseeModuleIndexViewModel : DashboardGridViewModelBase<Framework.DomainModel.Entities.FranchiseeModule>
    {
        public override string PageTitle
        {
            get
            {
                return "Set Franchisee's Modules";
            }
        }
    }
}