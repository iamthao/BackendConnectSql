using System.Collections.Generic;

namespace QuickspatchWeb.Models
{
    public class ExportExcel
    {
        public List<ColumnModel> GridConfigViewModel { get; set; }
        public List<dynamic> ListDataSource { get; set; }
    }
}