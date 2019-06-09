using System.Collections.Generic;
using System.Linq;
using Framework.DomainModel.ValueObject;

namespace QuickspatchWeb.Models
{
    public class ExportExcelModel
    {
        public List<ColumnModel> Columns { get; set; }

        public List<ReadOnlyGridVo> DataSource { get; set; }
    }

    public class ColumnModel
    {
        public string Field { get; set; }

        public string Title { get; set; }

        public string Type { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}