namespace QuickspatchWeb.Models.Module
{
    public class DashboardModuleIndexViewModel : DashboardGridViewModelBase<Framework.DomainModel.Entities.Module>
    {
        public override string PageTitle
        {
            get
            {
                return "Module";
            }
        }
    }
}