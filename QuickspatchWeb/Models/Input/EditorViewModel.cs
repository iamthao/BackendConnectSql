using System.Web.Routing;

namespace QuickspatchWeb.Models.Input
{
    public class EditorViewModel : ControlSharedViewModelBase
    {
        public string Class { get; set; }
        public string Style { get; set; }
        public bool Required { get; set; }
        public string RequiredAttribute
        {
            get
            {
                return Required ? "required=\"required\"" : "";
            }
        }
        public RouteValueDictionary HtmlAttributes { get; set; }

        public int Width { get; set; }
        public int Height { get; set; }
        public string UrlRead { get; set; }
        public string UrlDestroy { get; set; }
        public string UrlCreate { get; set; }
        public string UrlThumb { get; set; }
        public string UrlUpload { get; set; }
        public string UrlImage { get; set; }

    }
}