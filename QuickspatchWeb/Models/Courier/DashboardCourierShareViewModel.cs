using QuickspatchWeb.Models.User;
using System;
using System.Collections.Generic;

namespace QuickspatchWeb.Models.Courier
{
    public class DashboardCourierShareViewModel : DashboardSharedViewModel
    {
        public DashboardUserShareViewModel UserShareViewModel { get; set; }
        public byte Status { get; set; }
        public string CarNo { get; set; }
    }
}