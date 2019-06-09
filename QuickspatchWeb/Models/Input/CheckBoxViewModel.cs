using System.Web.Routing;

namespace QuickspatchWeb.Models.Input
{
    public class CheckBoxViewModel : ControlSharedViewModelBase
    {
        public string Class { get; set; }
        public string Style { get; set; }
        public RouteValueDictionary HtmlAttributes { get; set; }
        public string NgChangeFunction { get; set; }

        public bool IsDisabled { get; set; }
        public string MoreClass { get; set; }
        public bool LabelTop { get; set; }
    }
}