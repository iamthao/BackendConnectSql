using System.Collections.Generic;
using Framework.DomainModel.Entities;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface IRequestHistoryRepository: IRepository<RequestHistory>, IQueryableRepository<RequestHistory>
    {
        List<RequestHistory> GetHistoryRequestForToday(int userId, int utcClient);
    }
}
