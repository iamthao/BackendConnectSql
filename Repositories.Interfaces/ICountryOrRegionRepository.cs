using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Framework.DomainModel.Entities;
using Framework.Repositories;
using Framework.DomainModel.ValueObject;

namespace Repositories.Interfaces
{
    public interface ICountryOrRegionRepository : IRepository<CountryOrRegion>, IQueryableRepository<CountryOrRegion>
    {
        List<LookupItemVo> GetAllCountryOrRegionForLookUp();
    }
}
