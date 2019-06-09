using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickspatchWeb.Models.NoteRequest
{
    public class DashboardNoteRequestIndexViewModel : DashboardGridViewModelBase<Framework.DomainModel.Entities.NoteRequest>
    {
        public override string PageTitle
        {
            get
            {
                return "Request";
            }
            //test
        }
    }
}