using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickspatchWeb.Models.NoteRequest 
{
    public class DashboardNoteRequestDataViewModel : MasterfileViewModelBase<Framework.DomainModel.Entities.NoteRequest>
    {
        public override void MapFromClientParameters(MasterfileParameter parameters)
        {
            SharedViewModel = MapFromClientParameters<DashboardNoteRequestShareViewModel>(parameters);
        }
        //test
    }
}