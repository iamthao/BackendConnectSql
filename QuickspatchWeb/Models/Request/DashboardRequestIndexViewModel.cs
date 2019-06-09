namespace QuickspatchWeb.Models.Request
{
    public class DashboardRequestIndexViewModel : DashboardGridViewModelBase<Framework.DomainModel.Entities.Request>
    {
        public override string PageTitle
        {
            get
            {
                return "Request";
            }
        }
    }
}