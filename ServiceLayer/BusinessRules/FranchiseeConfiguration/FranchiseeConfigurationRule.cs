using System.Text.RegularExpressions;
using Framework.BusinessRule;
using Framework.DomainModel;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Framework.Service.Translation;
using Framework.Utility;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ServiceLayer.BusinessRules.FranchiseeConfiguration
{
    public class FranchiseeConfigurationRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {
        private readonly IFranchiseeConfigurationRepository _FranchiseeConfigurationRepository;

        public FranchiseeConfigurationRule(IFranchiseeConfigurationRepository FranchiseeConfigurationRepository)
        {
            _FranchiseeConfigurationRepository = FranchiseeConfigurationRepository;
        }

        public BusinessRuleResult Execute(IEntity instance)
        {
            var failed = false;
            var FranchiseeConfiguration = instance as Framework.DomainModel.Entities.FranchiseeConfiguration;
            var validationResult = new List<ValidationResult>();
            if (FranchiseeConfiguration != null)
            {
                if (!string.IsNullOrEmpty(FranchiseeConfiguration.Name) && _FranchiseeConfigurationRepository.CheckExist(o => o.Name == FranchiseeConfiguration.Name && o.Id != FranchiseeConfiguration.Id))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"), "Name");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                //if (string.IsNullOrEmpty(FranchiseeConfiguration.OfficePhone))
                //{
                //    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Office Phone");
                //    validationResult.Add(new ValidationResult(mess));
                //    failed = true;
                //}
                //else if (FranchiseeConfiguration.OfficePhone.Length < 10)
                //{
                //    var mess = string.Format(SystemMessageLookup.GetMessage("PhoneValid"), "Office Phone");
                //    validationResult.Add(new ValidationResult(mess));
                //    failed = true;
                //}
                //if (string.IsNullOrEmpty(FranchiseeConfiguration.FaxNumber))
                //{
                //    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Fax Number");
                //    validationResult.Add(new ValidationResult(mess));
                //    failed = true;
                //}
                //else if (FranchiseeConfiguration.FaxNumber.Length < 10)
                //{
                //    var mess = string.Format(SystemMessageLookup.GetMessage("PhoneValid"), "Fax Number");
                //    validationResult.Add(new ValidationResult(mess));
                //    failed = true;
                //}
                if (string.IsNullOrEmpty(FranchiseeConfiguration.Address1))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Address1");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                if (string.IsNullOrEmpty(FranchiseeConfiguration.City))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "City");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                if (string.IsNullOrEmpty(FranchiseeConfiguration.State))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "State");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (string.IsNullOrEmpty(FranchiseeConfiguration.Zip))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Zip");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                //FranchiseeConfiguration.Zip!="N/A" vi khi tao moi 1 franchisee khong co Address1, City, Zip, State
                //nen khi xuat hien man hinh Welcome Tour update franchisee khong duoc nen set tat cai la "N/A" o HomeController
                else if (FranchiseeConfiguration.Zip != "N/A")
                {
                    if (!Regex.IsMatch(FranchiseeConfiguration.Zip, @"^[0-9\\\-/]*$"))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("FieldInvalidText"), "Zip");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                }

                //if (string.IsNullOrEmpty(FranchiseeConfiguration.FranchiseeContact))
                //{
                //    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Contact Name");
                //    validationResult.Add(new ValidationResult(mess));
                //    failed = true;
                //}
                if (string.IsNullOrEmpty(FranchiseeConfiguration.PrimaryContactPhone))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Phone");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (FranchiseeConfiguration.PrimaryContactPhone.Length < 10)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("PhoneValid"), "Phone");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                //email
                if (string.IsNullOrEmpty(FranchiseeConfiguration.PrimaryContactEmail))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Email");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (Regex.IsMatch(FranchiseeConfiguration.PrimaryContactEmail, "[A-Z]"))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("EmailValid"), "Email");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (!EmailValidateHelper.IsValidEmail(FranchiseeConfiguration.PrimaryContactEmail))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("EmailValid"), "Email");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                if (!string.IsNullOrEmpty(FranchiseeConfiguration.PrimaryContactFax) && FranchiseeConfiguration.PrimaryContactFax.Length < 10)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("PhoneValid"), "Fax");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (string.IsNullOrEmpty(FranchiseeConfiguration.PrimaryContactCellNumber))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Cell");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (FranchiseeConfiguration.PrimaryContactCellNumber.Length < 10)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("PhoneValid"), "Cell");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
               

                if (string.IsNullOrEmpty(FranchiseeConfiguration.Logo.ToString()))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Logo");
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
            get { return "FranchiseeConfigurationRule"; }
        }

        public string[] PropertyNames { get; set; }
    }
}