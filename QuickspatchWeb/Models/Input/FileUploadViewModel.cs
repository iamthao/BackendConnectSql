namespace QuickspatchWeb.Models.Input
{
    public class FileUploadViewModel : ControlSharedViewModelBase
    {
        public bool Required { get; set; }

        public string RequiredAttribute
        {
            get
            {
                return Required ? "required=\"required\"" : "";
            }
        }

        public string SaveUrl { get; set; }

        public string RemoveUrl { get; set; }

        public int PreviewHeight { get; set; }

        public int PreviewWidth { get; set; }

        public string AcceptType { get; set; }

        public string FileNameSave { get; set; }

        public bool IsMultiple { get; set; }
        public bool IsAvatar { get; set; }
    }
}