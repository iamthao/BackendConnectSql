namespace QuickspatchWeb.Models.ModuleDocumentTypeOperation
{
    public class DashboardModuleDocumentTypeOperationDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.ModuleDocumentTypeOperation>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardModuleDocumentTypeOperationShareViewModel>(parameters);
        }
    }
}