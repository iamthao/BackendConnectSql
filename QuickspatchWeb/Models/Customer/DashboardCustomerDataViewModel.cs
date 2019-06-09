namespace QuickspatchWeb.Models.Customer
{
    public class DashboardCustomerDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.Customer>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardCustomerShareViewModel>(parameters);
        }
    }
}