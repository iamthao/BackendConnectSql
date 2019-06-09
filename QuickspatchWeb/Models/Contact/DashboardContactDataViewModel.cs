
namespace QuickspatchWeb.Models.Contact
{
    public class DashboardContactDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.Contact>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardContactShareViewModel>(parameters);
        }
    }
}