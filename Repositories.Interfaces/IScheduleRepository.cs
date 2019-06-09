using System;
using System.Collections.Generic;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.ValueObject;
using Framework.Repositories;

namespace Repositories.Interfaces
{
    public interface IScheduleRepository : IRepository<Schedule>, IQueryableRepository<Schedule>
    {
        dynamic GetSchedulesOfCourier(int courierId);
        dynamic GetDetailScheduleWeekly(int courierId, DateTime? fromDate, DateTime? toDate, int timezone);
        dynamic GetDetailScheduleMonthly(int courierId, DateTime? fromDate, DateTime? toDate, int timezone);

        List<Schedule> GetScheduleByCourier(int courierId, DateTime? durationEnd);
    }
}