using System.Web.Routing;

namespace QuickspatchWeb.Models.Input
{
    public class InputTextViewModel : ControlSharedViewModelBase
    {
        public string Class { get; set; }
        public string Style { get; set; }
        public bool Required { get; set; }
        public string PlaceHolderText { get; set; }
        public string RequiredAttribute
        {
            get
            {
                return Required ? "required=\"required\"" : "";
            }
        }
        public RouteValueDictionary HtmlAttributes { get; set; }
        public string TextboxType { get; set; }
        public bool IsDisabled { get; set; }
        public string MoreClass { get; set; }
        public string ButtonFunctionName { get; set; }
        public string ButtonFunctionText { get; set; }
        public string AutoComplete { get; set; }
    }
}