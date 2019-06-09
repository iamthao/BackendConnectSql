using System;
using Framework.BusinessRule;
using Framework.DomainModel;
using Framework.DomainModel.Entities;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.DomainModel.ValueObject;
using Framework.Service.Translation;
using Framework.Utility;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace ServiceLayer.BusinessRules.Franchisee
{
    public class FranchiseeRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {
        private readonly IFranchiseeTenantRepository _franchiseeTenantRepository;
        
        public FranchiseeRule(IFranchiseeTenantRepository franchiseeTenantRepository)
        {
            _franchiseeTenantRepository = franchiseeTenantRepository;
        }

        public BusinessRuleResult Execute(IEntity instance)
        {
            var failed = false;
            var franchisee = instance as Framework.DomainModel.Entities.FranchiseeTenant;            
            var validationResult = new List<ValidationResult>();
           // var franchiseeConfig = instance as Framework.DomainModel.Entities.FranchiseeConfiguration;
            

            if (franchisee != null)
            {
                if (string.IsNullOrEmpty(franchisee.Name))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Name");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (!string.IsNullOrEmpty(franchisee.Name) &&
                    _franchiseeTenantRepository.CheckExist(o => o.Name == franchisee.Name && o.Id != franchisee.Id))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"), "Name");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (string.IsNullOrEmpty(franchisee.Address1))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Address 1");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (string.IsNullOrEmpty(franchisee.City))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "City");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (string.IsNullOrEmpty(franchisee.State))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "State");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                int i = 0;
                if (string.IsNullOrEmpty(franchisee.Zip))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Zip");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (int.TryParse(franchisee.Zip, out i) == false)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("FieldInvalidText"), "Zip");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (string.IsNullOrEmpty(franchisee.OfficePhone))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Office Phone");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (franchisee.OfficePhone.Length < 10)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("PhoneValid"), "Office Phone");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (string.IsNullOrEmpty(franchisee.FaxNumber))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Fax Number");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (franchisee.FaxNumber.Length < 10)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("PhoneValid"), "Fax Number");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (string.IsNullOrEmpty(franchisee.Server))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Server");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (string.IsNullOrEmpty(franchisee.Database))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Database");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                if (string.IsNullOrEmpty(franchisee.UserName))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Username");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (string.IsNullOrEmpty(franchisee.Password))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Password");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                                             
                if (!string.IsNullOrEmpty(franchisee.LicenseKey) &&
                    _franchiseeTenantRepository.CheckExist(
                        o => o.LicenseKey == franchisee.LicenseKey && o.Id != franchisee.Id))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("ExistsTextResourceKey"), "License Key");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                //Primary Contact
                if (string.IsNullOrEmpty(franchisee.FranchiseeContact))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Frachisee Contact");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (string.IsNullOrEmpty(franchisee.PrimaryContactPhone))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Phone");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (franchisee.PrimaryContactPhone.Length < 10 || franchisee.PrimaryContactPhone.Contains("_"))
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("PhoneValid"), "Phone");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }

                if (!string.IsNullOrEmpty(franchisee.PrimaryContactEmail))
                {
                    if (Regex.IsMatch(franchisee.PrimaryContactEmail, "[A-Z]"))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("EmailValid"), "Email");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                    if (!EmailValidateHelper.IsValidEmail(franchisee.PrimaryContactEmail))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("EmailValid"), "Email");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                }

                if (!string.IsNullOrEmpty(franchisee.PrimaryContactFax))
                {

                    if (franchisee.PrimaryContactFax.Length < 10 || franchisee.PrimaryContactFax.Contains("_"))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("PhoneValid"), "Fax");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                }

                if (!string.IsNullOrEmpty(franchisee.PrimaryContactCellNumber))
                {


                    if (franchisee.PrimaryContactCellNumber.Length < 10 ||
                        franchisee.PrimaryContactCellNumber.Contains("_"))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("PhoneValid"), "Cell Number");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                }
                //license key
                if (franchisee.StartActiveDate == null)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Start Active Date");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (((DateTime)(franchisee.StartActiveDate)).Year == 1)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Start Active Date");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }


                if (franchisee.EndActiveDate == null)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "End Active Date");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                else if (((DateTime)(franchisee.EndActiveDate)).Year == 1)
                {
                    var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "End Active Date");
                    validationResult.Add(new ValidationResult(mess));
                    failed = true;
                }
                if (franchisee.EndActiveDate != null && franchisee.StartActiveDate != null)
                {
                    if (((DateTime)(franchisee.EndActiveDate)) < ((DateTime)(franchisee.StartActiveDate)))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("MustGreaterThan"), "End Active Date",
                            "StartActiveDate");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                }
                // Check connection string
                if (!string.IsNullOrEmpty(franchisee.Server) && !string.IsNullOrEmpty(franchisee.Database)
                    && !string.IsNullOrEmpty(franchisee.UserName) && !string.IsNullOrEmpty(franchisee.Password))
                {
                    var connectionString = PersistenceHelper.GenerateConnectionString(franchisee.Server,
                        franchisee.Database, franchisee.UserName, franchisee.Password);
                    if (!_franchiseeTenantRepository.CheckConnectionString(connectionString))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("ConnectionStringInvalid"));
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                }
                
                
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