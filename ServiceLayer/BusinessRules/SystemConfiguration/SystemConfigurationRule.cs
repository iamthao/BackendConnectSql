using System;
using Framework.BusinessRule;
using Framework.DomainModel;
using Framework.DomainModel.Entities.Common;
using Framework.DomainModel.Interfaces;
using Framework.Service.Translation;
using Repositories.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Framework.DomainModel.Entities;
using DataType = Framework.DomainModel.Entities.DataType;

namespace ServiceLayer.BusinessRules.SystemConfiguration
{
    public class SystemConfigurationRule<TEntity> : IBusinessRule<TEntity> where TEntity : Entity
    {
        private readonly ISystemConfigurationRepository _systemConfigurationRepository;

        public SystemConfigurationRule(ISystemConfigurationRepository systemConfigurationRepository)
        {
            _systemConfigurationRepository = systemConfigurationRepository;
        }
        public BusinessRuleResult Execute(IEntity instance)
        {
            var failed = false;
            var config = instance as Framework.DomainModel.Entities.SystemConfiguration;
            var validationResult = new List<ValidationResult>();
            if (config != null)
            {
                if (config.SystemConfigType == SystemConfigType.DispatchTimeDefault)
                {
                    if (String.IsNullOrEmpty(config.Value))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Value");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                    else if (config.DataType == DataType.Int)
                    {
                        int Num;
                        bool isNum = int.TryParse(config.Value, out Num);
                        if (!isNum)
                        {
                            var mess = string.Format(SystemMessageLookup.GetMessage("ValueIsInteger"), "Value");
                            validationResult.Add(new ValidationResult(mess));
                            failed = true;
                        }

                    }

                }
                else if (config.SystemConfigType == SystemConfigType.RequestNo)
                {
                    if (String.IsNullOrEmpty(config.Value))
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("RequiredTextResourceKey"), "Value");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }
                    else if (config.Value.Length > 3)
                    {
                        var mess = string.Format(SystemMessageLookup.GetMessage("LengthRequestNo"), "Value");
                        validationResult.Add(new ValidationResult(mess));
                        failed = true;
                    }

                }
                else if (config.SystemConfigType == SystemConfigType.DefaultLocationFrom)
                {
                    if (Convert.ToInt32(config.Value) > 0)
                    {
                        var to =
                            _systemConfigurationRepository.FirstOrDefault(
                                o => o.SystemConfigType == SystemConfigType.DefaultLocationTo);
                        if (to != null)
                        {
                            if (Convert.ToInt32(config.Value) == Convert.ToInt32(to.Value))
                            {
                                var mess = string.Format(SystemMessageLookup.GetMessage("FromDifferentTo"));
                                validationResult.Add(new ValidationResult(mess));
                                failed = true;
                            }
                        }
                        
                    }

                }
                else if (config.SystemConfigType == SystemConfigType.DefaultLocationTo)
                {
                    if (Convert.ToInt32(config.Value) > 0)
                    {
                        var from =
                            _systemConfigurationRepository.FirstOrDefault(
                                o => o.SystemConfigType == SystemConfigType.DefaultLocationFrom);
                        if (from != null)
                        {
                            if (Convert.ToInt32(config.Value) == Convert.ToInt32(from.Value))
                            {
                                var mess = string.Format(SystemMessageLookup.GetMessage("ToDefaultFrom"));
                                validationResult.Add(new ValidationResult(mess));
                                failed = true;
                            }
                        }

                    }

                }
                var result = new BusinessRuleResult(failed, "", instance.GetType().Name, instance.Id, PropertyNames, Name) { ValidationResults = validationResult };
                return result;
            }

            return new BusinessRuleResult();
        }

        public string Name
        {
            get { return "TemplateRule"; }
        }

        public string[] PropertyNames { get; set; }
    }
}