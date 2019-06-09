using System;
using Framework.BusinessRule;
using Framework.DomainModel;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.Service.Translation;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using Framework.Utility;


namespace ServiceLayer.BusinessRules.HoldingRequest
{
    public class HoldingRequestRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {
        private readonly IHoldingRequestRepository _holdingRequestRepository;

        public HoldingRequestRule(IHoldingRequestRepository holdingRequestRepository)
        {
            _holdingRequestRepository = holdingRequestRepository;
        }

        public BusinessRuleResult Execute(IEntity instance)
        {
            var failed = false;
            var holdingRequest = instance as Framework.DomainModel.Entities.HoldingRequest;
            var validationResult = new List<ValidationResult>();
            if (holdingRequest != null)
            {
                if (holdingRequest.LocationFrom == 0)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "From");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (holdingRequest.LocationTo == 0)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "To");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (holdingRequest.LocationFrom > 0 && holdingRequest.LocationTo > 0 && holdingRequest.LocationTo == holdingRequest.LocationFrom)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("MustDifferentResourceKey"), "From", "To");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                var now = DateTime.UtcNow.ToClientTimeDateTime();
                //Send Date


                if (holdingRequest.SendDate.GetValueOrDefault().Year == 1)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Dispatch Date");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (holdingRequest.SendDate.GetValueOrDefault() <= DateTime.MinValue)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("FieldInvalidText"), "Dispatch Date");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else
                {
                    if (holdingRequest.SendDate.GetValueOrDefault().ToClientTimeDateTime() <
                        new DateTime(now.Year, now.Month, now.Day, 0, 0, 0))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("MustGreaterThan"), "Dispatch Date", "Current Date");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }

                }

                // Time
                if (holdingRequest.StartTime == null || holdingRequest.StartTime <= DateTime.MinValue)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"),
                        "Arrival Window From");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else
                {
                    var sendDate = holdingRequest.SendDate.GetValueOrDefault().ToClientTimeDateTime();
                    if (new DateTime(sendDate.Year, sendDate.Month, sendDate.Day, 0, 0, 0) ==
                            new DateTime(now.Year, now.Month, now.Day, 0, 0, 0))
                    {
                        var startTime = holdingRequest.StartTime.GetValueOrDefault().ToClientTimeDateTime();
                        if (
                            new DateTime(now.Year, now.Month, now.Day, startTime.Hour,
                                startTime.Minute, 0) < now)
                        {
                            var mess = string.Format(SystemMessageLookup.GetMessage("MustGreaterThan"), "Arrival Window From", "Current Time");
                            validationResult.Add(new ValidationResult(mess));
                            failed = true;
                        }
                    }
                }

                if (holdingRequest.EndTime == null || holdingRequest.EndTime <= DateTime.MinValue)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Arrival Window To");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if ((holdingRequest.StartTime != null && holdingRequest.StartTime > DateTime.MinValue) && (holdingRequest.EndTime != null && holdingRequest.EndTime > DateTime.MinValue) && holdingRequest.StartTime >= holdingRequest.EndTime)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("MustGreaterThan"), "Arrival Window To", "Arrival Window From");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }




                //if (holdingRequest.StartTime == null)
                //{
                //    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Scheduled Departure Time");
                //    validationResult.Add(new ValidationResult(mess));
                //    failed = true;
                //}
                //if (holdingRequest.EndTime == null)
                //{
                //    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Scheduled Arrival Time");
                //    validationResult.Add(new ValidationResult(mess));
                //    failed = true;
                //}

                //if (holdingRequest.StartTime != null && holdingRequest.StartTime < DateTime.UtcNow)
                //{
                //    var mess = string.Format(SystemMessageLookup.GetMessage("MustGreaterThan"), "Scheduled Departure Time", "Current Time");
                //    validationResult.Add(new ValidationResult(mess));
                //    failed = true;
                //}

                //if (holdingRequest.StartTime != null && holdingRequest.EndTime != null && holdingRequest.StartTime >= holdingRequest.EndTime)
                //{
                //    var mess = string.Format(SystemMessageLookup.GetMessage("MustGreaterThan"), "Scheduled Arrival Time", "Scheduled Departure Time");
                //    validationResult.Add(new ValidationResult(mess));
                //    failed = true;
                //}

                var result = new BusinessRuleResult(failed, "", instance.GetType().Name, instance.Id, PropertyNames, Name) { ValidationResults = validationResult };
                return result;
            }

            return new BusinessRuleResult();
        }

        public string Name
        {
            get { return "ModuleRule"; }
        }

        public string[] PropertyNames { get; set; }
    }
}