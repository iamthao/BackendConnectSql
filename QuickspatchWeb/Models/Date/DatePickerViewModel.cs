using System;
using System.Web.Routing;

namespace QuickspatchWeb.Models.Date
{
    public class DatePickerViewModel : ControlSharedViewModelBase
    {
        public string Class { get; set; }
        public string Style { get; set; }
        public string Format { get; set; }
        public bool Required { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public string RequiredAttribute
        {
            get
            {
                return Required ? "required=\"required\"" : "";
            }
        }
        public RouteValueDictionary HtmlAttributes { get; set; }
    }
}