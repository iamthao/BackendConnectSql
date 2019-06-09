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

namespace ServiceLayer.BusinessRules.Contact
{
    public class ContactRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {

        private readonly IContactRepository _contactRepository;
        public ContactRule(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
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
            var contact = instance as Framework.DomainModel.Entities.Contact;
            var validationResult = new List<ValidationResult>();
            if (contact != null)
            {
                if (string.IsNullOrEmpty(contact.Name.Trim()))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Name");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (!string.IsNullOrEmpty(contact.Name) && _contactRepository.CheckExist(o => o.Name == contact.Name && o.Id != contact.Id))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"), "Name");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }



                if (string.IsNullOrEmpty(contact.Phone))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Phone");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (contact.Phone.Length < 10)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("PhoneValid"), "Phone");
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
            get { return "ContactRule"; }
        }

        public string[] PropertyNames { get; set; }
    }
}