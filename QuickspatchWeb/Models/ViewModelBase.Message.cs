using Framework.Service.Translation;

namespace QuickspatchWeb.Models
{
    public partial class ViewModelBase
    {
        public string CreateText
        {
            get { return SystemMessageLookup.GetMessage("CreateText"); }
        }
        public string UpdateText
        {
            get { return SystemMessageLookup.GetMessage("UpdateText"); }
        }
        public string DirtyDialogMessageText
        {
            get { return SystemMessageLookup.GetMessage("DirtyDialogMessageText"); }
        }
    }
}