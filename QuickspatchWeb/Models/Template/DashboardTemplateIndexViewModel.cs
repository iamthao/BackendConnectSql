namespace QuickspatchWeb.Models.Template
{
    public class DashboardTemplateIndexViewModel : DashboardGridViewModelBase<Framework.DomainModel.Entities.Template>
    {
        public override string PageTitle
        {
            get
            {
                return "Template";
            }
        }
    }
}