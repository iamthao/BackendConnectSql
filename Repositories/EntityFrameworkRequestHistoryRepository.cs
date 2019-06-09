using System;
using System.Collections.Generic;
using System.Linq;
using Database.Persistance.Tenants;
using Framework.DomainModel.DataTransferObject;
using Framework.DomainModel.Entities;
using Repositories.Interfaces;

namespace Repositories
{
    public class EntityFrameworkRequestHistoryRepository : EntityFrameworkTenantRepositoryBase<RequestHistory>, IRequestHistoryRepository
    {
        public EntityFrameworkRequestHistoryRepository(ITenantPersistenceService persistenceService) : base(persistenceService)
        {
        }

        public List<RequestHistory> GetHistoryRequestForToday(int userId, int utcClient)
        {
            var currentDateParse = DateTime.UtcNow;
            var clientDate = DateTime.UtcNow.AddMilliseconds(utcClient);
            var currentPrevious = currentDateParse.AddMilliseconds(-1 *
                                                 clientDate.Subtract(new DateTime(clientDate.Year, clientDate.Month, clientDate.Day)).TotalMilliseconds);
            //var currentPrevious = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day).AddSeconds(-1 * utcClient);
            var finalResult =
                GetAll()
                    .Where(
                        o => o.Request.CreatedOn != null && o.Request.CreatedOn <= currentDateParse && o.Request.CreatedOn >= currentPrevious &&
                             o.CourierId == userId);
            return finalResult.ToList();
        }
    }
    
}
