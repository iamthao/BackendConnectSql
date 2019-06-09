using System.Collections.Generic;
using System.Web.Routing;

namespace QuickspatchWeb.Models
{
    public class LookupViewModel
    {

        public string ID { get; set; }
        public string ModelName { get; set; }
        public string Label { get; set; }
        public string UrlToReadData { get; set; }
        public string UrlToGetLookupItem { get; set; }
        public RouteValueDictionary HtmlAttributes { get; set; }
        public int CurrentId { get; set; }
        public string HierarchyGroupName { get; set; }
        public bool PopulatedByChildren { get; set; }
        public List<object> DataSource { get; set; }
        public int HeightLookup { get; set; }
        public string DataBindingValue { get; set; }
        public bool Required { get; set; }
        public string AngularLookupController { get; set; }
        public string RequiredAttribute
        {
            get
            {
                return Required ? "required=\"required\"" : "";
            }
        }

        public int Col { get; set; }

        public string CustomParams { get; set; }

        public string IsShow { get; set; }

        public string AddLookupPopupFunction { get; set; }
        public string EditLookupPopupFunction { get; set; }
        public string MoreClass { get; set; }
        public bool ShowAddEdit { get; set; }
        public string Enable { get; set; }
    }
}