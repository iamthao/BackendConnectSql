using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QuickspatchWeb.Models.Request;
using QuickspatchWeb.Models.Courier;

namespace QuickspatchWeb.Models.NoteRequest
{
    public class DashboardNoteRequestShareViewModel : DashboardSharedViewModel
    {
        public DashboardRequestShareViewModel UserShareViewModel { get; set; }
        public DashboardCourierShareViewModel CourierShareViewModel { get; set; }
        public string Descripttion { get; set; }
        //test
    }
}
