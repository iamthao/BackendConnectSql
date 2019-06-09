using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DomainModel.Entities;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer.Interfaces
{
    public interface ICountryOrRegionService : IMasterFileService<CountryOrRegion>
    {
        List<LookupItemVo> GetAllCountryOrRegionForLookUp();
    }
}
