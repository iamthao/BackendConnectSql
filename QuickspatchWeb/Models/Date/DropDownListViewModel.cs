using System.Collections.Generic;

namespace QuickspatchWeb.Models.Date
{
    public class DropDownListViewModel : ControlSharedViewModelBase
    {
        public bool Required { get; set; }
        public string RequiredAttribute
        {
            get
            {
                return Required ? "required=\"required\"" : "";
            }
        }
        public object CurrentValue { get; set; }

        public string ReadUrl { get; set; }

        public string MoreClass { get; set; }

        public string EventChange { get; set; }
    }
}