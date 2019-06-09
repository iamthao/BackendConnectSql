using System;
using System.Linq.Expressions;
using System.Web.Mvc;
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
using ServiceLayer.Interfaces;

namespace ServiceLayer.BusinessRules.Request
{
    public class RequestRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IFranchiseeTenantService _franchiseeTenantService;

        public RequestRule(IRequestRepository requestRepository, IFranchiseeTenantService franchiseeTenantService)
        {
            _requestRepository = requestRepository;
            _franchiseeTenantService = franchiseeTenantService;
        }
        private void ValidateFieldLength(ref bool failed, ref List<ValidationResult> validationResult, string field,
            int acceptedLength, string nameValidate)
        {
            if (String.IsNullOrEmpty(field) || field.Length == acceptedLength) return;

            var mess = string.Format(SystemMessageLookup.GetMessage("FieldInvalidText"), nameValidate);
            validationResult.Add(new ValidationResult(mess));
            failed = true;
        }

        public BusinessRuleResult Execute(IEntity instance)
        {
            var failed = false;
            var request = instance as Framework.DomainModel.Entities.Request;
            var validationResult = new List<ValidationResult>();

            if (request != null)
            {
                if (request.LocationFrom<=0)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "From");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;

                }

                if (request.LocationTo <= 0)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "To");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;

                }

                if (request.LocationFrom>0 &&request.LocationTo> 0&& request.LocationTo == request.LocationFrom)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("MustDifferentResourceKey"), "From", "To");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (request.CourierId ==null||request.CourierId == 0)
                {

                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), _franchiseeTenantService.GetDisplayNameForCourier());
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;

                }
                if (request.SendingTime == null && request.IsStat == false)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Dispatch Time");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else
                {
                    if (request.SendingTime < DateTime.UtcNow && request.IsStat == false)
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("MustGreaterThan"), "Dispatch Time", "Current Time");
                           validationResult.Add(new ValidationResult(mess));
                            failed = true;
                    }
                }
                if (request.StartTime.Year==1)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Arrival Window From");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;

                }
                if (request.EndTime.Year==1)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Arrival Window To");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
               
                if (request.StartTime.Year != 1 && request.SendingTime != null && request.SendingTime > request.StartTime)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("MustGreaterThan"), "Arrival Window From", "Dispatch Time");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                //if (request.StartTime.Year != 1 && request.EndTime.Year != 1 && request.StartTime < DateTime.UtcNow)
                //{
                //    var mess = string.Format(SystemMessageLookup.GetMessage("MustGreaterThan"), "Arrival Window From", "Current Time");
                //    validationResult.Add(new ValidationResult(mess));
                //    failed = true;
                //}

                //if (request.StartTime.Year != 1 && request.EndTime.Year != 1 && request.EndTime < DateTime.UtcNow)
                //{
                //    var mess = string.Format(SystemMessageLookup.GetMessage("MustGreaterThan"), "Arrival Window To", "Current Time");
                //    validationResult.Add(new ValidationResult(mess));
                //    failed = true;
                //}

                if (request.StartTime.Year != 1 && request.EndTime.Year != 1 && request.EndTime <= request.StartTime )
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
            get { return "requestRule"; }
        }

        public string[] PropertyNames { get; set; }
    }
}
