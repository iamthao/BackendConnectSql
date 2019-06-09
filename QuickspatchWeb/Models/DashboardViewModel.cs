using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickspatchWeb.Models
{
    public class DashboardViewModel
    {
        public virtual IEnumerable<Framework.DomainModel.Entities.Request> Requests { get; set; }
    }
}