using System.Collections.Generic;
using Framework.DomainModel.Entities;
using Framework.Repositories;
using Framework.DomainModel.ValueObject;

namespace Repositories.Interfaces
{
    public interface ILocationRepository :IRepository<Location>, IQueryableRepository<Location>
    {
        Location GetLocation();
        List<LocationDefaultVo> GetListLocation();

        LatLngVo GetLatLng(int fromId);
    }
}
