using System.Web.Routing;

namespace QuickspatchWeb.Models.Input
{
    public class InputMaskedViewModel : ControlSharedViewModelBase
    {
        public string Format { get; set; }
        public RouteValueDictionary HtmlAttributes { get; set; }
        public bool Required { get; set; }
        public string RequiredAttribute
        {
            get
            {
                return Required ? "required=\"required\"" : "";
            }
        }

        public string PlaceHolderText { get; set; }
        public string MoreClass { get; set; }
    }
}