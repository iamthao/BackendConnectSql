﻿using Framework.BusinessRule;
using Framework.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using Framework.Service.Translation;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.Utility;
using System.Text.RegularExpressions;
using ServiceLayer.Common;

namespace ServiceLayer.BusinessRules.Courier
{
    public class CourierRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {
        private readonly ICourierRepository _courierRepository;
        private readonly IUserRepository _userRepository;
        public CourierRule(ICourierRepository courierRepository, IUserRepository userRepository)
        {
            _courierRepository= courierRepository;
            _userRepository = userRepository;
        }

        private void ValidateFieldLength(ref bool failed, ref List<ValidationResult> validationResult, string field, int acceptedLength, string nameValidate)
        {
            if (String.IsNullOrEmpty(field) || field.Length == acceptedLength) return;

            var mess = string.Format(SystemMessageLookup.GetMessage("FieldInvalidText"), nameValidate);
            validationResult.Add(new ValidationResult(mess));
            failed = true;
        }

        public BusinessRuleResult Execute(IEntity instance)
        {
            var failed = false;
            var courier = instance as Framework.DomainModel.Entities.Courier;
            var validationResult = new List<ValidationResult>();

            if (courier != null)
            {
                var user = courier.User;

                //validate licence
                //if (courier.Id == 0 && MenuExtractData.Instance.NumberOfCourier != null && MenuExtractData.Instance.NumberOfCourier != 0 && _courierRepository.ListAll().Count >= MenuExtractData.Instance.NumberOfCourier)
                //{
                //    var mess = string.Format(SystemMessageLookup.GetMessage("MaximumOfCourierExceeded"));
                //    validationResult.Add(new ValidationResult(mess));
                //    failed = true;
                //}
                //else
                //{
                    //username
                    if (string.IsNullOrEmpty(user.UserName))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Username");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                    else if (!string.IsNullOrEmpty(user.UserName) &&
                            _userRepository.CheckExist(o => o.UserName == user.UserName && o.Id != user.Id))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"), "Username");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                    //email
                    if (string.IsNullOrEmpty(user.Email))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Email");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                    else if (Regex.IsMatch(user.Email, "[A-Z]"))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("EmailValid"), "Email");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                    else if (!EmailValidateHelper.IsValidEmail(user.Email))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("EmailValid"), "Email");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                    else if (!string.IsNullOrEmpty(user.Email) &&
                            _userRepository.CheckExist(
                                o => o.Email.Trim().ToLower() == user.Email.Trim().ToLower() && o.Id != user.Id))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"), "Email");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                    //firstName
                    if (string.IsNullOrEmpty(courier.User.FirstName))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "First Name");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }

                    if (string.IsNullOrEmpty(user.LastName))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Last Name");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }

                    if (string.IsNullOrEmpty(user.HomePhone))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Home Phone");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                    else if (user.HomePhone.Length < 10)
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("PhoneValid"), "Home Phone");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }


                    if (string.IsNullOrEmpty(user.MobilePhone))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Mobile Phone");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                    else if (user.MobilePhone.Length < 10)
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("PhoneValid"), "Mobile Phone");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }

                    if (string.IsNullOrEmpty(courier.CarNo))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Car Number");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                //}
                
                
                var result = new BusinessRuleResult(failed, "", instance.GetType().Name, instance.Id, PropertyNames, Name) { ValidationResults = validationResult };
                return result;
                
            }

            return new BusinessRuleResult();

        }

        public string Name
        {
            get { return "CourierRule"; }
        }

        public string[] PropertyNames { get; set; }
    }
}