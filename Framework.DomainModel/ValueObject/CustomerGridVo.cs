using System.Collections.Generic;

namespace Framework.DomainModel.ValueObject
{
    public class CustomerGridVo : ReadOnlyGridVo
    {
        public string Name { get; set; }
    }

    public class MasterfileGridDataVo
    {
        public IList<ReadOnlyGridVo> Data { get; set; }

        public long TotalRowCount { get; set; }
    }

}