using System;
using QuickspatchWeb.Models.NoteRequest;
namespace QuickspatchWeb.Models.HoldingRequest
{
    public class DashboardHoldingRequestShareViewModel : DashboardSharedViewModel
    {
        public DashboardNoteRequestShareViewModel NoteRequestShareViewModel { get; set; }
        public int? LocationFrom { get; set; }
        public string LocationFromName { get; set; }
        public int? LocationTo { get; set; }
        public string LocationToName { get; set; }
        public string Description { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? SendDate { get; set; }
    }
}