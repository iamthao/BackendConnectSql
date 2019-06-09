namespace QuickspatchWeb.Models.HoldingRequest
{
    public class DashboardHoldingRequestIndexViewModel : DashboardGridViewModelBase<Framework.DomainModel.Entities.HoldingRequest>
    {
        public override string PageTitle
        {
            get
            {
                return "Holding Request";
            }
        }
    }
}