using QuickspatchWeb.Models.NoteRequest;
using System;
using System.Collections.Generic;

namespace QuickspatchWeb.Models.Request
{
    public class DashboardRequestShareViewModel : DashboardSharedViewModel
    {
        public DashboardNoteRequestShareViewModel NoteRequestShareViewModel { get; set; }
        public int? LocationFrom { get; set; }
        public int? LocationTo { get; set; }
        public int? CourierId { get; set; }
        public bool? IsStat { get; set; }
        public DateTime? SendingTime { get; set; }
        public string Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? Status { get; set; }
        public int? ExpiredTime { get; set; }
        public bool? Confirm { get; set; }
        public int? HoldingRequestId { get; set; }
        //public string DisplayNameForCourier { get; set; }
    }
}