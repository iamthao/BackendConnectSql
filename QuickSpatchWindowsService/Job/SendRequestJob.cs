using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Autofac;
using Framework.DomainModel.Common;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.Service.Diagnostics;
using Framework.Utility;
using ServiceLayer;
using ServiceLayer.Interfaces;

namespace QuickSpatchWindowsService.Job
{
    public class SendRequestJob
    {
        public static void SendRequestJobThread(object state)
        {
            var diagnosticService = Program.Container.Resolve<IDiagnosticService>();
            var scheduleService = Program.Container.Resolve<IScheduleService>();
            var requestService = Program.Container.Resolve<IRequestService>();
           
            
            var currentDate = DateTime.UtcNow;
            var listRequestInsert = new List<Request>();
            try
            {
                var startOfDate = currentDate.Date.AddDays(-1);
                var endOfdate = currentDate.Date.AddTicks(-1);
                var listSchedule = scheduleService.ListAll();
                foreach (var schedule in listSchedule.Where(schedule => schedule.DurationStart <= endOfdate.AddMinutes(schedule.TimeZone.GetValueOrDefault()) && 
                    ((schedule.DurationEnd != null && (!(schedule.IsNoDurationEnd ?? false)) && schedule.DurationEnd >= startOfDate.AddMinutes(schedule.TimeZone.GetValueOrDefault()))
                        || (schedule.IsNoDurationEnd ?? false))))
                {
                    currentDate = DateTime.UtcNow.AddMinutes(schedule.TimeZone.GetValueOrDefault());

                    var t1 = new TimeSpan(DateTime.UtcNow.Hour, DateTime.UtcNow.Minute, DateTime.UtcNow.Second);
                    var t2 = new TimeSpan(schedule.EndTime.AddMinutes(schedule.TimeZone.GetValueOrDefault()).Hour, schedule.EndTime.AddMinutes(schedule.TimeZone.GetValueOrDefault()).Minute, schedule.EndTime.AddMinutes(schedule.TimeZone.GetValueOrDefault()).Second);

                    //diagnosticService.Info("End: " + t2.TotalSeconds + " Now: " + t1.TotalSeconds + " Name: " + schedule.Name);
                    if (t2.TotalSeconds > t1.TotalSeconds)
                    {
                        string[] fre;
                        switch (CronTriggerHelper.FrequencySelected(schedule.Frequency))
                        {
                            case "daily":
                                listRequestInsert.Add(CreateRequest(schedule));
                                break;

                            case "weekly":

                                fre = CronTriggerHelper.FrequencyWeek(schedule.Frequency).Split(',');
                                if (fre.Contains(currentDate.DayOfWeek.ToString().Substring(0, 3).ToUpper()))
                                {
                                    listRequestInsert.Add(CreateRequest(schedule));
                                }

                                break;

                            case "monthly":
                                fre = CronTriggerHelper.FrequencyMonth(schedule.Frequency).Split(',');
                                if (fre.Length >= 1 && !string.IsNullOrEmpty(fre[0]))
                                {
                                    int[] convertedItems = Array.ConvertAll<string, int>(fre, int.Parse);
                                    if (convertedItems.Contains(currentDate.Day))
                                    {
                                        listRequestInsert.Add(CreateRequest(schedule));
                                    }

                                }

                                break;
                            default:
                                break;
                        }
                    }

                    requestService.AddListRequestForService(listRequestInsert);
                }
            }
            catch (Exception ex)
            {
                diagnosticService.Error(ex);
            }
        }

        private static Request CreateRequest(Schedule schedule)
        {
            var locationService = Program.Container.Resolve<ILocationService>();
            var googleService = Program.Container.Resolve<IGoogleService>();

            var endOfDate = DateTime.UtcNow.AddMinutes(schedule.TimeZone.GetValueOrDefault()).Date.AddDays(1).AddTicks(-1);

            var expiredTime = Math.Round((endOfDate - DateTime.UtcNow.AddMinutes(schedule.TimeZone.GetValueOrDefault())).TotalSeconds);

            return (new Request()
            {
                CourierId = schedule.CourierId,
                CreatedById = schedule.CreatedById,
                Description = schedule.Description,
                EndTime = schedule.EndTime,
                LocationTo = schedule.LocationTo,
                LocationFrom = schedule.LocationFrom,
                StartTime = schedule.StartTime,
                SendingTime = DateTime.UtcNow,
                Status = (int)StatusRequest.NotSent,
                HistoryScheduleId = schedule.Id,
                ExpiredTime = expiredTime,
            });
        }
    }
}
