
using System;
using System.Collections.Generic;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;

namespace ServiceLayer.Interfaces
{
    public interface IScheduleService : IMasterFileService<Schedule>
    {

        dynamic GetSchedulesOfCourier(int courierId);
        dynamic GetDetailScheduleWeekly(int courierId, DateTime? fromDate, DateTime? toDate, int timezone);
        dynamic GetDetailScheduleMonthly(int courierId, DateTime? fromDate, DateTime? toDate, int timezone);

        WarningScheduleVo GetWarningInfo(int scheduleId, int courierId);
        bool DeleteSchedule(int id);
    }
}