namespace QuickspatchWeb.Models.Report
{
    public class PrintPdfFileViewModel
    {
        public string FileName { get; set; }
        public string EmailSubject { get; set; }
        public string Path { get; set; }
        public string Email { get; set; }
        public string FullPath
        {
            get { return Path + FileName; }
        }
    }
}