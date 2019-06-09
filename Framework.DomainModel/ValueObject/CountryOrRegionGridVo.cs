using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.DomainModel.ValueObject
{
    public class CountryOrRegionGridVo: ReadOnlyGridVo
    {
        public string Name { get; set; }
        public string CodeName { get; set; }
    }
}
