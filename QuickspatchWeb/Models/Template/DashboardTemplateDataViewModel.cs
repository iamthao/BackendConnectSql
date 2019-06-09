namespace QuickspatchWeb.Models.Template
{
    public class DashboardTemplateDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.Template>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardTemplateShareViewModel>(parameters);
        }

        
    }
}