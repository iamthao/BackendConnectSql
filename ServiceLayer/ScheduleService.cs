using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Mvc;
using Database.Persistance.Tenants;
using Framework.BusinessRule;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Framework.Service.Translation;
using Framework.Utility;
using Repositories.Interfaces;
using ServiceLayer.Interfaces;

namespace ServiceLayer
{
    public class ScheduleService : MasterFileService<Schedule>, IScheduleService
    {
        private readonly IScheduleRepository _scheduleRepository;

        public ScheduleService(ITenantPersistenceService tenantPersistenceService, IScheduleRepository scheduleRepository,
            IBusinessRuleSet<Schedule> businessRuleSet = null)
            : base(scheduleRepository, scheduleRepository, tenantPersistenceService, businessRuleSet)
        {
            _scheduleRepository = scheduleRepository;
        }

        public dynamic GetSchedulesOfCourier(int courierId)
        {
            return _scheduleRepository.GetSchedulesOfCourier(courierId);
        }

        public dynamic GetDetailScheduleWeekly(int courierId, DateTime? fromDate, DateTime? toDate, int timezone)
        {
            return _scheduleRepository.GetDetailScheduleWeekly(courierId, fromDate, toDate, timezone);
        }

        public dynamic GetDetailScheduleMonthly(int courierId, DateTime? fromDate, DateTime? toDate, int timezone)
        {
            return _scheduleRepository.GetDetailScheduleMonthly(courierId, fromDate, toDate, timezone);
        }

        public override Schedule Add(Schedule entity)
        {
            ValidateBusinessRules(entity);
            entity.DurationStart = entity.DurationStart.ToUtcTimeFromClientTime();
            if (entity.DurationEnd != null)
            {
                entity.DurationEnd = entity.DurationEnd.Value.ToUtcTimeFromClientTime();
            }
            entity.TimeZone = DateTimeHelper.GetClientTimeZone();

            WarningScheduleVo warningScheduleVo;
            var listSchedule = WarningProcess(entity, out warningScheduleVo, false, 0);
            var result = listSchedule.FirstOrDefault(o => o.Id == entity.Id);
            if (result != null && result.IsWarning.GetValueOrDefault() && !result.Confirm.GetValueOrDefault())
            {
                warningScheduleVo.IsUpdate = false;
                result.WarningInfo = warningScheduleVo;
                return result;
            }

            var listScheduleFull = Get(o => o.CourierId == entity.CourierId).ToList();
            var listNotInListSchedule = listScheduleFull.Except(listSchedule);
            foreach (var schedule in listNotInListSchedule)
            {
                schedule.IsWarning = false;
                _scheduleRepository.Update(schedule);
            }
            foreach (var schedule in listSchedule)
            {
                if (schedule.Id == 0)
                {
                    result = schedule;
                    _scheduleRepository.Add(result);
                }
                else
                {
                    _scheduleRepository.Update(schedule);
                }
            }
            _scheduleRepository.Commit();
            return result;
            //return base.Add(entity);
        }

        public override Schedule Update(Schedule entity)
        {
            ValidateBusinessRules(entity);
            entity.DurationStart = entity.DurationStart.ToUtcTimeFromClientTime();
            if (entity.DurationEnd != null)
            {
                entity.DurationEnd = entity.DurationEnd.Value.ToUtcTimeFromClientTime();
            }
            entity.TimeZone = DateTimeHelper.GetClientTimeZone();

            WarningScheduleVo warningScheduleVo;
            var listSchedule = WarningProcess(entity, out warningScheduleVo, false, 0);
            var result = listSchedule.FirstOrDefault(o => o.Id == entity.Id);
            if (result != null && result.IsWarning.GetValueOrDefault() && !result.Confirm.GetValueOrDefault())
            {
                warningScheduleVo.IsUpdate = false;
                result.WarningInfo = warningScheduleVo;
                return result;
            }

            var listScheduleFull = Get(o => o.CourierId == entity.CourierId).ToList();
            var listNotInListSchedule = listScheduleFull.Except(listSchedule);
            foreach (var schedule in listNotInListSchedule)
            {
                schedule.IsWarning = false;
                _scheduleRepository.Update(schedule);
            }

            foreach (var schedule in listSchedule)
            {
                if (schedule.Id == 0)
                {
                    result = schedule;
                    _scheduleRepository.Update(result);
                }
                else
                {
                    _scheduleRepository.Update(schedule);
                }
            }
            _scheduleRepository.Commit();
            return result;
            //return base.Update(entity);
        }

        public bool DeleteSchedule(int id)
        {
            var entity = _scheduleRepository.GetById(id);
            WarningScheduleVo warningScheduleVo;
            var listSchedule = WarningProcess(entity, out warningScheduleVo, true, id);
            foreach (var schedule in listSchedule)
            {
                _scheduleRepository.Update(schedule);
            }
            entity.IsDeleted = true;
            _scheduleRepository.Update(entity);
            _scheduleRepository.Commit();
            return true;
        }

        private List<Schedule> WarningProcess(Schedule entity, out WarningScheduleVo warningScheduleVo, bool isDelete, int idDelete)
        {

            var offset = DateTimeHelper.GetClientTimeZone();
            var currentDateParse = DateTime.UtcNow;
            var clientDate = currentDateParse.AddMinutes(offset);
            var startNowTime = currentDateParse.AddMilliseconds(-1 *
                                                 clientDate.Subtract(new DateTime(clientDate.Year, clientDate.Month, clientDate.Day)).TotalMilliseconds);
            warningScheduleVo = null;

            var listSchedule = _scheduleRepository.GetScheduleByCourier(entity.CourierId, entity.DurationEnd);

            if (listSchedule.Count == 0)
            {
                return new List<Schedule> { entity };
            }

            //With Create

            if (entity.Id == 0)
            {
                listSchedule.Add(entity);
            }
            //With Update
            else
            {
                Func<Schedule, bool> condition = o => o.Id == entity.Id;
                if (listSchedule.FirstOrDefault(condition) != null)
                {
                    listSchedule.First(condition).Name = entity.Name;
                    listSchedule.First(condition).LocationFrom = entity.LocationFrom;
                    listSchedule.First(condition).LocationTo = entity.LocationTo;
                    listSchedule.First(condition).Frequency = entity.Frequency;
                    listSchedule.First(condition).StartTime = entity.StartTime;
                    listSchedule.First(condition).EndTime = entity.EndTime;
                    listSchedule.First(condition).DurationStart = entity.DurationStart;
                    listSchedule.First(condition).DurationEnd = entity.DurationEnd;
                    listSchedule.First(condition).IsNoDurationEnd = entity.IsNoDurationEnd;
                    listSchedule.First(condition).Description = entity.Description;
                    listSchedule.First(condition).TimeZone = entity.TimeZone;
                }
            }

            foreach (var item in listSchedule)
            {
                if (item.Id == 0)
                {
                    item.CopyCreatedOn = DateTime.UtcNow;
                }
                else
                {
                    item.CopyCreatedOn = item.CreatedOn;
                }
            }

            listSchedule = listSchedule.Where(w => w.IsNoDurationEnd == true || DateTimeHelper.CompareDateTime(w.DurationEnd.GetValueOrDefault(), startNowTime) >= 0)
                .OrderBy(o => o.StartTime.ToString("yyyyMMddHHmm"))
                .ThenBy(o => o.CopyCreatedOn.GetValueOrDefault().ToString("yyyyMMddHHmm"))
                .ToList();
            Schedule preivousSchedule = null;
            foreach (var schedule in listSchedule)
            {
                if (schedule.Id == idDelete && isDelete)
                {
                    schedule.IsDeleted = true;
                }
                else
                {
                    if (preivousSchedule != null && !schedule.IsDeleted)
                    {
                        schedule.IsWarning = CheckStartTimeAndTime(preivousSchedule, schedule);
                        if (schedule.IsWarning.GetValueOrDefault())
                        {
                            if (schedule.Id == entity.Id)
                            {
                                warningScheduleVo = new WarningScheduleVo
                                {
                                    PreviousName = preivousSchedule.Name,
                                    PreviousStartTime = preivousSchedule.StartTime,
                                    PreviousEndTime = preivousSchedule.EndTime,
                                    Name = schedule.Name,
                                    StartTime = schedule.StartTime,
                                    EndTime = schedule.EndTime,
                                };
                            }
                        }
                    }
                    else if (preivousSchedule == null && !schedule.IsDeleted)
                    {
                        schedule.IsWarning = false;
                    }
                    preivousSchedule = schedule;
                }

            }
            return listSchedule;
        }

        private bool CheckStartTimeAndTime(Schedule preivousSchedule, Schedule schedule)
        {
            var startTimePreviousClient = preivousSchedule.StartTime.ToClientTimeDateTime();
            var endTimePreviousClient = preivousSchedule.EndTime.ToClientTimeDateTime();
            var startTimeClient = schedule.StartTime.ToClientTimeDateTime();

            var startTimePrevious = new DateTime(startTimePreviousClient.Year, startTimePreviousClient.Month,
                                    startTimePreviousClient.Day, startTimePreviousClient.Hour, startTimePreviousClient.Minute, 0); //preivousSchedule.StartTime.ToString("HH:mm");
            var endTimePrevious = new DateTime(startTimePreviousClient.Year, startTimePreviousClient.Month,
                                   startTimePreviousClient.Day, endTimePreviousClient.Hour, endTimePreviousClient.Minute, 0);//preivousSchedule.EndTime.ToString("HH:mm");
            var startTime = new DateTime(startTimePreviousClient.Year, startTimePreviousClient.Month,
                                    startTimePreviousClient.Day, startTimeClient.Hour, startTimeClient.Minute, 0);//schedule.StartTime.ToString("HH:mm");


            //var startTimePrevious = new DateTime(preivousSchedule.StartTime.Year, preivousSchedule.StartTime.Month,
            //                        preivousSchedule.StartTime.Day, preivousSchedule.StartTime.Hour, preivousSchedule.StartTime.Minute,0); //preivousSchedule.StartTime.ToString("HH:mm");
            //var endTimePrevious = new DateTime(preivousSchedule.EndTime.Year, preivousSchedule.EndTime.Month,
            //                        preivousSchedule.EndTime.Day, preivousSchedule.EndTime.Hour, preivousSchedule.EndTime.Minute, 0);//preivousSchedule.EndTime.ToString("HH:mm");
            //var startTime = new DateTime(preivousSchedule.StartTime.Year, preivousSchedule.StartTime.Month,
            //                        preivousSchedule.StartTime.Day, schedule.StartTime.Hour, schedule.StartTime.Minute, 0);//schedule.StartTime.ToString("HH:mm");
            //var endTime = new DateTime(preivousSchedule.EndTime.Year, preivousSchedule.EndTime.Month,
            //                        preivousSchedule.EndTime.Day, schedule.EndTime.Hour, schedule.EndTime.Minute, 0);//schedule.EndTime.ToString("HH:mm");

            var result = false;
            //A Nghiep
            //result = System.String.CompareOrdinal(endTimePrevious, startTime) >= 0 ||
            //         System.String.CompareOrdinal(endTimePrevious, endTime) <= 0;
            result = DateTimeHelper.CompareDateTime(startTime, startTimePrevious) >= 0
                     && DateTimeHelper.CompareDateTime(endTimePrevious, startTime) >= 0;//startTimePrevious <= startTime && startTime <= endTimePrevious;  //Thao


            //A Nghiep
            //result = System.String.CompareOrdinal(startTimePrevious, startTime) >= 0 ||
            //         System.String.CompareOrdinal(startTimePrevious, endTime) <= 0;
            return result;
        }

        public WarningScheduleVo GetWarningInfo(int scheduleId, int courierId)
        {
            var listSchedule =
                _scheduleRepository.Get(o => o.CourierId == courierId)
                    .OrderBy(o => o.StartTime.ToString("yyyyMMddHHmm"))
                    .ThenBy(o => o.CreatedOn.GetValueOrDefault().ToString("yyyyMMddHHmm"))
                    .ToList();
            Schedule preivousSchedule = null;
            var result = new WarningScheduleVo();
            foreach (var schedule in listSchedule)
            {
                if (preivousSchedule != null)
                {
                    if (schedule.Id == scheduleId)
                    {
                        return new WarningScheduleVo
                        {
                            PreviousName = preivousSchedule.Name,
                            PreviousStartTime = preivousSchedule.StartTime,
                            PreviousEndTime = preivousSchedule.EndTime,
                            Name = schedule.Name,
                            StartTime = schedule.StartTime,
                            EndTime = schedule.EndTime,
                        };
                    }
                }
                preivousSchedule = schedule;
                result = new WarningScheduleVo
                {
                    PreviousName = preivousSchedule.Name,
                    PreviousStartTime = preivousSchedule.StartTime,
                    PreviousEndTime = preivousSchedule.EndTime,
                    Name = schedule.Name,
                    StartTime = schedule.StartTime,
                    EndTime = schedule.EndTime,
                };
            }
            return result;
        }


        /// <summary>
        /// 0. None, 1. Daily, 2. Weekly, 3.Monthly
        /// </summary>
        /// <param name="frequency"></param>
        /// <returns></returns>
        private int GetFrequencyType(string frequency)
        {
            var fre = frequency.Split(' ');
            if (fre.Length > 0)
            {
                if (fre.Length == 6 && fre[3] == "*" && fre[4] == "*" && fre[5] == "?")
                {
                    return 1;
                }
                if (fre.Length == 6 && fre[3] != "*" && fre[4] == "*" && fre[5] == "?")
                {
                    return 3;
                }
                return 2;
            }
            return 0;
        }
    }
}