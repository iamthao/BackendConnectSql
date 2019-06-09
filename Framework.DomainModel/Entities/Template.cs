using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Framework.DomainModel.Entities
{
    public class Template : Entity
    {

        public string Title { get; set; }
        public string Content { get; set; }
        public int TemplateType { get; set; }
        public ReportType? ReportType { get; set; }
    }
    public enum ReportType : byte
    {     
        [Description("Report")]
        Report = 1,
        
    }
}
