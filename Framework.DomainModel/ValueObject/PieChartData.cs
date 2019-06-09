using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.ValueObject
{
    public class PieChartData
    {
        public string category { get; set; }
        public int value { get; set; }
        public string color { get; set; }
        public bool? selected { get; set; }
    }
}
