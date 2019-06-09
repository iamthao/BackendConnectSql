using System;
using System.Configuration;
using Framework.BusinessRule;
using Framework.DomainModel;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.Service.Translation;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Framework.Utility;

namespace ServiceLayer.BusinessRules.User
{
    public class UserRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {
        private readonly IUserRepository _userRepository;

        public UserRule(IUserRepository userRepository)
        {
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
            var user = instance as Framework.DomainModel.Entities.User;
            var validationResult = new List<ValidationResult>();
            if (user != null)
            {             
                if (!string.IsNullOrEmpty(user.UserRoleId.ToString()) && user.UserRoleId == 0)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Role");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (!string.IsNullOrEmpty(user.UserName) && _userRepository.CheckExist(o => o.UserName == user.UserName && o.Id != user.Id))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"), "Username");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                //firstName
                if (string.IsNullOrEmpty(user.FirstName))
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

                
                var result = new BusinessRuleResult(failed, "", instance.GetType().Name, instance.Id, PropertyNames, Name) { ValidationResults = validationResult };
                return result;
            }

            return new BusinessRuleResult();
        }

        public string Name
        {
            get { return "UserRule"; }
        }

        public string[] PropertyNames { get; set; }
    }
}