using System;
using System.Web.Mvc;
using Framework.BusinessRule;
using Framework.DomainModel;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.Service.Translation;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Framework.Utility;
using System.Text.RegularExpressions;

namespace ServiceLayer.BusinessRules.Location
{
    public class LocationRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {
        private readonly ILocationRepository _locationRepository;

        public LocationRule(ILocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
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
            var location = instance as Framework.DomainModel.Entities.Location;
            var validationResult = new List<ValidationResult>();
            int i = 0;
            if (location != null)
            {
                if (string.IsNullOrEmpty(location.Name))
                {
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"),
                            "Location Name");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                }
                else if (_locationRepository.CheckExist(o => o.Name == location.Name.Trim() && o.Id != location.Id))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"), "Location Name");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                

                if (string.IsNullOrEmpty(location.Address1))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Address1");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;

                }

                if (string.IsNullOrEmpty(location.City))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "City");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;

                }

                if (string.IsNullOrEmpty(location.StateOrProvinceOrRegion))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "State / Province / Region");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;

                } 

                if (string.IsNullOrEmpty(location.Zip))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Zip");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (int.TryParse(location.Zip, out i)== false)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("FieldInvalidText"), "Zip");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }


                if (location.IdCountryOrRegion == 0)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Country / Region");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (location.CloseHour < location.OpenHour && location.CloseHour !=null)
                {
                    
                    var mess = string.Format(SystemMessageLookup.GetMessage("MustGreaterThan"), "Close Hour","Open Hour");
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
            get { return "locationRule"; }
        }

        public string[] PropertyNames { get; set; }
    }
}
