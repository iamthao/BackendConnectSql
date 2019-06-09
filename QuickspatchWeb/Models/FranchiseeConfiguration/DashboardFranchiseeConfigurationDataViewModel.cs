namespace QuickspatchWeb.Models.FranchiseeConfiguration
{
    public class DashboardFranchiseeConfigurationDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.FranchiseeConfiguration>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardFranchiseeConfigurationShareViewModel>(parameters);
        }
    }

    public class LicenceExtensionData
    {
        public string KeyCode { get; set; }
        public string UserName { get; set; }
        public int PackageId { get; set; }
    }
}