using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QuickspatchWeb.Models.Report
{
    public class DriverReportPdfViewModel
    {
        public string FileName { get; set; }
        public string Path { get; set; }
        public string FullPath
        {
            get { return Path + FileName; }
        }
    }
}