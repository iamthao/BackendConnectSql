namespace QuickspatchWeb.Models.Customer
{
    public class DashboardCustomerIndexViewModel : DashboardGridViewModelBase<Framework.DomainModel.Entities.Customer>
    {
        public override string PageTitle
        {
            get
            {
                return "Customer";
            }
        }
        
    }
}