using System;
using Framework.BusinessRule;
using Framework.DomainModel;
using Framework.DomainModel.Common;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.Service.Translation;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Framework.Utility;

namespace ServiceLayer.BusinessRules.Schedule
{
    public class ScheduleRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {

        private readonly IScheduleRepository _scheduleRepository;
        private readonly ICourierRepository _courierRepository;

        public ScheduleRule(IScheduleRepository scheduleRepository, ICourierRepository courierRepository)
        {
            _scheduleRepository = scheduleRepository;
            _courierRepository = courierRepository;
        }
        public BusinessRuleResult Execute(IEntity instance)
        {
            var failed = false;
            var schedule = instance as Framework.DomainModel.Entities.Schedule;
            var validationResult = new List<ValidationResult>();
            if (schedule != null)
            {
                if (!string.IsNullOrEmpty(schedule.Name) && _scheduleRepository.CheckExist(o => o.Name == schedule.Name && o.Id != schedule.Id && o.CourierId == schedule.CourierId))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"), "Router Name");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (schedule.CourierId == 0)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("NotSelectCourierYet"));
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (schedule.CourierId > 0)
                {
                    var courier = _courierRepository.GetById(schedule.CourierId);
                    if (courier == null)
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("CourierIsDeleted"),"Courier");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                    else if (courier != null && !courier.User.IsActive)
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("InacticeCourier"));
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                  
                }
                

                if (schedule.LocationFrom == 0)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "From");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (schedule.LocationTo == 0)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "To");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (string.IsNullOrEmpty(schedule.Frequency))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Frequency");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (!string.IsNullOrEmpty(schedule.Frequency) && string.IsNullOrEmpty(schedule.Frequency.Split(' ')[3]))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredSelectValue"), "days of month");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (!string.IsNullOrEmpty(schedule.Frequency) && string.IsNullOrEmpty(schedule.Frequency.Split(' ')[5]))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredSelectValue"), "date of week");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (schedule.LocationFrom > 0 && schedule.LocationTo > 0 && schedule.LocationTo == schedule.LocationFrom)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("MustDifferentResourceKey"), "From", "To");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }


                var offset = DateTimeHelper.GetClientTimeZone();
                var currentDateParse = DateTime.UtcNow;
                var clientDate = currentDateParse.AddMinutes(offset);
                var startNowTime = currentDateParse.AddMilliseconds(-1 *
                                                         clientDate.Subtract(new DateTime(clientDate.Year, clientDate.Month, clientDate.Day)).TotalMilliseconds);

                if (schedule.DurationStart.Year==1)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Start Date");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (schedule.DurationStart <= DateTime.MinValue)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("FieldInvalidText"), "Start Date");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else
                {
                    if (schedule.Id == 0 && schedule.DurationStart < startNowTime)
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("MustGreaterThanOrEqualTo"), "Start Date", "Current Date");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                }

                if (!(schedule.IsNoDurationEnd ?? false) && schedule.DurationEnd.GetValueOrDefault().Year == 1)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "End Date");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (!(schedule.IsNoDurationEnd ?? false)  && schedule.DurationEnd <= DateTime.MinValue)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("FieldInvalidText"), "End Date");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (schedule.DurationStart != null && schedule.DurationStart > DateTime.MinValue
                    && (!(schedule.IsNoDurationEnd ?? false) && schedule.DurationEnd != null && schedule.DurationEnd > DateTime.MinValue) 
                    && schedule.DurationStart > schedule.DurationEnd)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("MustGreaterThanOrEqualTo"), "End Date", "Start Date");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else
                {
                    if (schedule.Id > 0 && schedule.IsNoDurationEnd != true && schedule.DurationEnd < startNowTime && schedule.DurationEnd > DateTime.MinValue)
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("MustGreaterThanOrEqualTo"), "End Date", "Current Date");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                }

                if (schedule.StartTime == null || schedule.StartTime <= DateTime.MinValue)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Arrival Window From");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (schedule.EndTime == null || schedule.EndTime <= DateTime.MinValue)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Arrival Window To");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if ((schedule.StartTime != null && schedule.StartTime > DateTime.MinValue) && (schedule.EndTime != null && schedule.EndTime > DateTime.MinValue) && schedule.StartTime >= schedule.EndTime)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("MustGreaterThan"), "Arrival Window To", "Arrival Window From");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                var result = new BusinessRuleResult(failed, "", instance.GetType().Name, instance.Id, PropertyNames, Name) { ValidationResults = validationResult };
                return result;
            }

            return new BusinessRuleResult();
        }

        public string Name
        {
            get { return "ScheduleRule"; }
        }

        public string[] PropertyNames { get; set; }
    }
}