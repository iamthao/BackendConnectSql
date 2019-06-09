using System;
using QuickspatchWeb.Models.NoteRequest;
namespace QuickspatchWeb.Models.Request
{
    public class SendHoldingRequestViewModel : DashboardSharedViewModel
    {
        public int CourierId { get; set; }
        public DateTime? SendingTime { get; set; }
        public bool IsStat { get; set; }
        public bool AutoAssign { get; set; }
    }
}