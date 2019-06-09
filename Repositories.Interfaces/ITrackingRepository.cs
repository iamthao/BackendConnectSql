using Framework.DomainModel.Entities;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface ITrackingRepository : IRepository<Tracking>, IQueryableRepository<Tracking>
    {
    }
}